using BackupAutomaticoCervantes.DestinoBackup.Factory;
using BackupAutomaticoCervantes.DestinoBackup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackupAutomaticoCervantes.Padrao;

namespace BackupAutomaticoCervantes.Services
{
    /// <summary>
    /// Serviço principal para execução de backups
    /// Demonstra o uso do Factory Pattern para criar destinos de backup
    /// </summary>
    public class BackupService
    {
        private readonly IBackupDestinoFactory _destinoFactory;
        private readonly string _logPath;

        /// <summary>
        /// Construtor do serviço de backup
        /// </summary>
        /// <param name="destinoFactory">Factory para criação de destinos (pode ser injetada via DI)</param>
        public BackupService(IBackupDestinoFactory destinoFactory = null)
        {
            // Se não fornecida, usa a implementação padrão
            _destinoFactory = destinoFactory ?? new BackupDestinoFactory();
            _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "backup.log");
        }

        /// <summary>
        /// Executa backup para múltiplos destinos configurados
        /// Demonstra como a factory simplifica a criação de diferentes tipos de destino
        /// </summary>
        /// <param name="parametros">Parâmetros do backup incluindo destinos</param>
        /// <param name="caminhoArquivoBackup">Caminho do arquivo de backup gerado</param>
        /// <returns>Lista de resultados por destino</returns>
        public async Task<List<ResultadoEnvioBackup>> EnviarBackupParaDestinosAsync(
            ParametrosBackupModel parametros,
            string caminhoArquivoBackup)
        {
            var resultados = new List<ResultadoEnvioBackup>();

            // Valida se o arquivo de backup existe
            if (!File.Exists(caminhoArquivoBackup))
            {
                var erro = $"Arquivo de backup não encontrado: {caminhoArquivoBackup}";
                EscreverLog($"ERRO: {erro}");
                throw new FileNotFoundException(erro);
            }

            EscreverLog($"Iniciando envio do backup '{Path.GetFileName(caminhoArquivoBackup)}' para {parametros.Destinos.Count} destino(s)");

            // Itera sobre todos os destinos configurados
            foreach (var configDestino in parametros.Destinos)
            {
                var resultado = new ResultadoEnvioBackup
                {
                    DestinoId = configDestino.Id,
                    TipoDestino = configDestino.Tipo,
                    IniciadoEm = DateTime.Now
                };

                try
                {
                    EscreverLog($"Enviando para destino {configDestino.Tipo} (ID: {configDestino.Id})...");

                    // AQUI ESTÁ O USO DO FACTORY PATTERN
                    // A factory se encarrega de criar a instância correta baseada na configuração
                    // O código cliente não precisa saber como criar cada tipo específico
                    IBackupDestino destino = _destinoFactory.CriarDestino(configDestino);

                    // Executa o envio - polimorfismo em ação
                    await destino.EnviarBackupAsync(caminhoArquivoBackup);

                    // Sucesso
                    resultado.Sucesso = true;
                    resultado.FinalizadoEm = DateTime.Now;
                    EscreverLog($"✅ Backup enviado com sucesso para {configDestino.Tipo}");
                }
                catch (Exception ex)
                {
                    // Erro no envio
                    resultado.Sucesso = false;
                    resultado.MensagemErro = ex.Message;
                    resultado.FinalizadoEm = DateTime.Now;

                    EscreverLog($"❌ Erro ao enviar backup para {configDestino.Tipo}: {ex.Message}");

                    // Não interrompe o processo para outros destinos
                    // Permite que outros destinos ainda sejam tentados
                }

                resultados.Add(resultado);
            }

            // Log consolidado
            var sucessos = resultados.FindAll(r => r.Sucesso).Count;
            var erros = resultados.Count - sucessos;
            EscreverLog($"Envio finalizado. Sucessos: {sucessos}, Erros: {erros}");

            return resultados;
        }

        /// <summary>
        /// Método auxiliar para demonstrar como criar um destino específico usando a factory
        /// Útil para testes ou configurações manuais
        /// </summary>
        /// <param name="tipo">Tipo do destino desejado</param>
        /// <returns>Configuração de exemplo para o tipo especificado</returns>
        public DestinoConfig CriarConfiguracaoExemplo(DestinoTipo tipo)
        {
            var config = new DestinoConfig { Tipo = tipo };

            // Cria configurações de exemplo baseadas no tipo
            // Em um cenário real, essas configurações viriam do usuário/banco de dados
            switch (tipo)
            {
                case DestinoTipo.Ftp:
                    config.Ftp = new DestinoBackup.Ftp.FtpConfig
                    {
                        Url = "ftp://exemplo.com/backup/",
                        Usuario = "usuario_ftp",
                        Senha = "senha_ftp"
                    };
                    break;

                case DestinoTipo.S3:
                    config.S3 = new DestinoBackup.Amazon.S3Config
                    {
                        AccessKey = "SUA_ACCESS_KEY",
                        SecretKey = "SUA_SECRET_KEY",
                        BucketName = "meu-bucket-backup",
                        Region = "us-east-1"
                    };
                    break;

                case DestinoTipo.GoogleDrive:
                    config.GoogleDrive = new DestinoBackup.GoogleDrive.GoogleDriveConfig
                    {
                        CredenciaisJsonPath = "caminho/para/credenciais.json"
                    };
                    break;

                case DestinoTipo.OneDrive:
                    config.OneDrive = new DestinoBackup.OneDrive.OneDriveConfig
                    {
                        TenantId = "seu-tenant-id",
                        ClientId = "seu-client-id",
                        ClientSecret = "seu-client-secret"
                    };
                    break;
            }

            return config;
        }

        /// <summary>
        /// Testa se um destino específico está configurado corretamente
        /// Demonstra validação usando a factory
        /// </summary>
        /// <param name="config">Configuração do destino a ser testada</param>
        /// <returns>True se a configuração é válida</returns>
        public async Task<bool> TestarConfiguracaoDestinoAsync(DestinoConfig config)
        {
            try
            {
                // Tenta criar o destino usando a factory
                // Se houver problema na configuração, a factory lançará exceção
                var destino = _destinoFactory.CriarDestino(config);

                EscreverLog($"✅ Configuração do destino {config.Tipo} validada com sucesso");
                return true;
            }
            catch (Exception ex)
            {
                EscreverLog($"❌ Erro na configuração do destino {config.Tipo}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Escreve log das operações
        /// </summary>
        /// <param name="mensagem">Mensagem a ser logada</param>
        private void EscreverLog(string mensagem)
        {
            ClsStatica.EscreverLog($"[BackupService] {mensagem}", _logPath);
        }
    }
}
