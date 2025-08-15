// Services/FotoMigrationService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PostgresImageMigration.Models;
using PostgresImageMigration.Repositories;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration.Services
{
    /// <summary>
    /// Serviço responsável pela leitura das imagens e envio ao repositório.
    /// </summary>
    public class FotoMigrationServiceOriginal
    {
        private readonly IFotoRepositoryOriginal _repository;
        private readonly string _diretorioImagens = @"C:\Users\Robson\Desktop\AtualizarCadastroAutomático\BancoDeDadosSupermercado\Imagens\all_imagens";
        private readonly int _tamanhoLote = 1000; // Ajuste conforme memória e performance

        public FotoMigrationServiceOriginal()
        {
            _repository = new FotoRepositoryOriginal();
        }

        /// <summary>
        /// Processa as imagens do diretório em lotes para evitar sobrecarga de memória.
        /// </summary>
        public void MigrarImagens()
        {
            if (!Directory.Exists(_diretorioImagens))
            {
                Logger.Log($"Diretório não encontrado: {_diretorioImagens}");
                return;
            }

            var arquivos = Directory.EnumerateFiles(_diretorioImagens)
                                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                                    .ToList();

            Logger.Log($"Total de imagens encontradas: {arquivos.Count}");

            int processadas = 0;
            foreach (var lote in DividirEmLotes(arquivos, _tamanhoLote))
            {
                var fotos = new List<Foto>(_tamanhoLote);
                foreach (var arquivo in lote)
                {
                    try
                    {
                        string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                        byte[] conteudo = File.ReadAllBytes(arquivo); // Leitura rápida para byte[]

                        fotos.Add(new Foto
                        {
                            Identificacao = nomeArquivo,
                            Conteudo = conteudo
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Erro ao ler arquivo {arquivo}: {ex.Message}");
                    }
                }

                _repository.InserirEmLote(fotos);
                processadas += fotos.Count;
                Logger.Log($"Progresso: {processadas}/{arquivos.Count}");
            }
        }

        /// <summary>
        /// Divide uma lista em lotes menores.
        /// </summary>
        private static IEnumerable<List<T>> DividirEmLotes<T>(List<T> source, int tamanhoLote)
        {
            for (int i = 0; i < source.Count; i += tamanhoLote)
            {
                yield return source.GetRange(i, Math.Min(tamanhoLote, source.Count - i));
            }
        }
    }
}
