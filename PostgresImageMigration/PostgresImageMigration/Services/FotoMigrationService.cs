using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using PostgresImageMigration.Models;
using PostgresImageMigration.Repositories;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Services
{
    /// <summary>
    /// Serviço que executa toda a migração: leitura paralela, pipeline e escrita via COPY.
    /// Implementa IDisposable para garantir fechamento correto da conexão.
    /// </summary>
    public class FotoMigrationService : IDisposable
    {
        private readonly string _diretorioImagens;
        private readonly HashSet<string> _extensoesPermitidas;
        private readonly int _tamanhoLote;
        private readonly int _filaCapacidade;
        private readonly int _grauParalelismoLeitura;
        private readonly string _erroLotesDir;

        private readonly BlockingCollection<Foto> _filaFotos;
        private PostgresSessionManager _pgSessionManager;
        private IFotoRepository _fotoRepository;
        private bool _disposed;

        public FotoMigrationService()
        {
            _diretorioImagens = ConfigurationManager.AppSettings["Imagens_Diretorio"];
            var extcfg = ConfigurationManager.AppSettings["Imagens_Extensoes"] ?? ".jpg,.jpeg,.png";
            _extensoesPermitidas = new HashSet<string>(extcfg.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToLower()));

            _tamanhoLote = int.Parse(ConfigurationManager.AppSettings["Tamanho_Lote"] ?? "5000");
            _filaCapacidade = int.Parse(ConfigurationManager.AppSettings["Fila_Capacidade"] ?? ((_tamanhoLote * 3).ToString()));
            _grauParalelismoLeitura = int.Parse(ConfigurationManager.AppSettings["Grau_Paralelismo_Leitura"] ?? "4");
            _erroLotesDir = ConfigurationManager.AppSettings["Erro_Lotes_Dir"] ?? "ErrorBatches";

            // Inicializa fila com capacidade limitada para conter memória
            _filaFotos = new BlockingCollection<Foto>(_filaCapacidade);
        }

        /// <summary>
        /// Ponto de entrada que executa a migração.
        /// </summary>
        public void MigrarImagens()
        {
            if (string.IsNullOrEmpty(_diretorioImagens) || !Directory.Exists(_diretorioImagens))
            {
                Logger.Log("[Service] Diretório de imagens inválido: " + _diretorioImagens);
                return;
            }

            // Lista de arquivos filtrados por extensão
            var arquivos = Directory.EnumerateFiles(_diretorioImagens)
                .Where(path => _extensoesPermitidas.Contains(Path.GetExtension(path).ToLower()))
                .ToList();

            Logger.Log($"[Service] Imagens encontradas: {arquivos.Count}");
            if (arquivos.Count == 0) return;

            // Abre a conexão (persistente) e aplica SETs na sessão
            _pgSessionManager = new PostgresSessionManager();
            _pgSessionManager.Open(); // abre e aplica sessão turbo

            // Cria repository com a conexão aberta
            _fotoRepository = new FotoRepository(_pgSessionManager.Connection);

            // Tarefa do escritor que consome a fila e insere por lotes
            var writerTask = Task.Factory.StartNew(() =>
            {
                ProcessWriter();
            }, TaskCreationOptions.LongRunning);

            // Leitura paralela dos arquivos (somente leitura de disco e criação de objetos Foto)
            var paraleloOptions = new ParallelOptions { MaxDegreeOfParallelism = _grauParalelismoLeitura };
            try
            {
                Parallel.ForEach(arquivos, paraleloOptions, arquivo =>
                {
                    try
                    {
                        // Lê file bytes (IO bound). Em C# 7.3 não usamos async/await no Parallel.ForEach.
                        var bytes = File.ReadAllBytes(arquivo);
                        var nome = Path.GetFileNameWithoutExtension(arquivo);

                        var foto = new Foto
                        {
                            Identificacao = nome,
                            Conteudo = bytes
                        };

                        // Adiciona à fila (bloqueante se a fila estiver cheia)
                        _filaFotos.Add(foto);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("[Service] Erro ao ler arquivo: " + arquivo, ex);
                    }
                });
            }
            catch (AggregateException aex)
            {
                Logger.LogException("[Service] Erros durante leitura paralela", aex);
            }
            finally
            {
                // Indica que não haverá mais itens
                _filaFotos.CompleteAdding();
            }

            // Aguarda writer terminar
            writerTask.Wait();

            Logger.Log("[Service] Escrita concluída.");
        }

        /// <summary>
        /// Faz a montagem de lotes e chama o repository para inserir via COPY.
        /// Se um lote falhar, salva o lote em disco (pasta ErrorBatches) para reprocessamento manual posterior.
        /// </summary>
        private void ProcessWriter()
        {
            var lote = new List<Foto>(_tamanhoLote);
            int totalInseridos = 0;
            try
            {
                foreach (var foto in _filaFotos.GetConsumingEnumerable())
                {
                    lote.Add(foto);

                    if (lote.Count >= _tamanhoLote)
                    {
                        TrySalvarLote(lote, ref totalInseridos);
                        lote.Clear();
                    }
                }

                // salva restante
                if (lote.Count > 0)
                {
                    TrySalvarLote(lote, ref totalInseridos);
                    lote.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("[Service] Erro no writer", ex);
            }

            Logger.Log($"[Service] Total aproximado inserido: {totalInseridos}");
        }

        /// <summary>
        /// Tenta inserir um lote; em caso de exceção salva o lote no disco para reprocessamento.
        /// </summary>
        private void TrySalvarLote(List<Foto> lote, ref int totalInseridos)
        {
            try
            {
                _fotoRepository.InserirEmLote(lote);
                totalInseridos += lote.Count;
                Logger.Log($"[Service] Lote inserido. Total até agora: {totalInseridos}");
            }
            catch (Exception ex)
            {
                Logger.LogException("[Service] Erro ao inserir lote via COPY", ex);

                // Em caso de erro, salva o lote em disco para possível reprocessamento manual
                try
                {
                    SaveFailedBatchToDisk(lote, ex);
                }
                catch (Exception sex)
                {
                    Logger.LogException("[Service] Falha ao salvar lote com erro no disco", sex);
                }
            }
        }

        /// <summary>
        /// Salva um lote que falhou em um arquivo binário + um arquivo .txt de metadados para reprocessamento.
        /// Struture: ErrorBatches\batch_TIMESTAMP.bin  + batch_TIMESTAMP_meta.txt
        /// </summary>
        private void SaveFailedBatchToDisk(List<Foto> lote, Exception ex)
        {
            try
            {
                Directory.CreateDirectory(_erroLotesDir);
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                var metaPath = Path.Combine(_erroLotesDir, $"batch_{timestamp}_meta.txt");
                var binPath = Path.Combine(_erroLotesDir, $"batch_{timestamp}.bin");

                // Salva metadados (err) e número de itens
                using (var sw = new StreamWriter(metaPath, false))
                {
                    sw.WriteLine("Erro ao inserir lote via COPY:");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine();
                    sw.WriteLine("Itens do lote:");
                    foreach (var f in lote)
                    {
                        sw.WriteLine(f.Identificacao);
                    }
                }

                // Salva binário concatenado: um formato simples que grava: [len:int][nameLength:int][name UTF8 bytes][contentLength:int][content bytes]...
                using (var fs = new FileStream(binPath, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    foreach (var f in lote)
                    {
                        var nameBytes = System.Text.Encoding.UTF8.GetBytes(f.Identificacao ?? string.Empty);
                        bw.Write(nameBytes.Length);
                        bw.Write(nameBytes);
                        bw.Write(f.Conteudo?.Length ?? 0);
                        if (f.Conteudo != null && f.Conteudo.Length > 0)
                            bw.Write(f.Conteudo);
                    }
                }

                Logger.Log($"[Service] Lote com erro salvo em disco: {metaPath} / {binPath}");
            }
            catch (Exception saveEx)
            {
                Logger.LogException("[Service] Erro ao salvar lote de erro no disco", saveEx);
            }
        }

        /// <summary>
        /// Dispose garante fechamento da conexão e limpeza.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                if (_pgSessionManager != null)
                {
                    _pgSessionManager.Dispose();
                    _pgSessionManager = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("[Service] Erro ao descartar PostgresSessionManager", ex);
            }
        }
    }
}
