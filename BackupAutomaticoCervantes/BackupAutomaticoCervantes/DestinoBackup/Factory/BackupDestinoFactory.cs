using BackupAutomaticoCervantes.DestinoBackup.Amazon;
using BackupAutomaticoCervantes.DestinoBackup.Ftp;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.DestinoBackup.OneDrive;
using System;

namespace BackupAutomaticoCervantes.DestinoBackup.Factory
{
    /// <summary>
    /// Factory responsável por criar instâncias de destinos de backup
    /// Implementa o padrão Factory Method para encapsular a lógica de criação
    /// </summary>
    public class BackupDestinoFactory : IBackupDestinoFactory
    {
        /// <summary>
        /// Cria uma instância de IBackupDestino baseada na configuração fornecida
        /// </summary>
        /// <param name="configuracao">Configuração do destino contendo tipo e parâmetros específicos</param>
        /// <returns>Instância concreta do destino de backup</returns>
        /// <exception cref="ArgumentNullException">Quando a configuração é nula</exception>
        /// <exception cref="NotSupportedException">Quando o tipo de destino não é suportado</exception>
        /// <exception cref="InvalidOperationException">Quando a configuração específica está ausente</exception>
        public IBackupDestino CriarDestino(IDestinoConfig configuracao)
        {
            // Validação de entrada
            if (configuracao == null)
                throw new ArgumentNullException(nameof(configuracao), "A configuração do destino não pode ser nula");

            // Switch expression para determinar o tipo de destino a ser criado
            // Cada caso valida a configuração específica e retorna a instância apropriada
            switch (configuracao.Tipo)
            {
                case DestinoTipo.Ftp:
                    return CriarDestinoFtp(configuracao);
                case DestinoTipo.GoogleDrive:
                    return CriarDestinoGoogleDrive(configuracao);
                case DestinoTipo.S3:
                    return CriarDestinoS3(configuracao);
                case DestinoTipo.OneDrive:
                    return CriarDestinoOneDrive(configuracao);
                default:
                    throw new NotSupportedException($"Tipo de destino '{configuracao.Tipo}' não é suportado.");
            }
        }

        /// <summary>
        /// Cria instância específica para destino FTP
        /// </summary>
        /// <param name="configuracao">Configuração contendo dados FTP</param>
        /// <returns>Instância de FtpBackupDestino configurada</returns>
        private IBackupDestino CriarDestinoFtp(IDestinoConfig configuracao)
        {
            var ftpConfig = configuracao as FtpConfig
                ?? throw new InvalidOperationException("Configuração inválida para destino FTP.");

            ValidarCampoObrigatorio(ftpConfig.Url, nameof(ftpConfig.Url));
            ValidarCampoObrigatorio(ftpConfig.Usuario, nameof(ftpConfig.Usuario));
            ValidarCampoObrigatorio(ftpConfig.Senha, nameof(ftpConfig.Senha));

            return new FtpBackupDestino();
        }

        /// <summary>
        /// Cria instância específica para destino Google Drive
        /// </summary>
        /// <param name="configuracao">Configuração contendo dados Google Drive</param>
        /// <returns>Instância de GoogleDriveBackupDestino configurada</returns>
        private IBackupDestino CriarDestinoGoogleDrive(IDestinoConfig configuracao)
        {
            // Valida se a configuração Google Drive está presente
            var googleDriveConfig = configuracao as GoogleDriveConfig
                ?? throw new InvalidOperationException("Configuração inválida para destino Google Drive.");

            // Google Drive precisa do ID do destino para autenticação isolada
            return new GoogleDriveBackupDestino(googleDriveConfig);
        }

        /// <summary>
        /// Cria instância específica para destino Amazon S3
        /// </summary>
        /// <param name="configuracao">Configuração contendo dados S3</param>
        /// <returns>Instância de AmazonS3BackupDestino configurada</returns>
        private IBackupDestino CriarDestinoS3(IDestinoConfig configuracao)
        {
            var s3Config = configuracao as S3Config
                ?? throw new InvalidOperationException("Configuração inválida para destino S3.");

            ValidarCampoObrigatorio(s3Config.AccessKey, nameof(s3Config.AccessKey));
            ValidarCampoObrigatorio(s3Config.SecretKey, nameof(s3Config.SecretKey));
            ValidarCampoObrigatorio(s3Config.BucketName, nameof(s3Config.BucketName));
            ValidarCampoObrigatorio(s3Config.Region, nameof(s3Config.Region));

            return new AmazonS3BackupDestino();
        }

        /// <summary>
        /// Cria instância específica para destino OneDrive
        /// </summary>
        /// <param name="configuracao">Configuração contendo dados OneDrive</param>
        /// <returns>Instância de OneDriveBackupDestino configurada</returns>
        private IBackupDestino CriarDestinoOneDrive(IDestinoConfig configuracao)
        {
            var oneDriveConfig = configuracao as OneDriveConfig
                ?? throw new InvalidOperationException("Configuração inválida para destino OneDrive.");

            ValidarCampoObrigatorio(oneDriveConfig.TenantId, nameof(oneDriveConfig.TenantId));
            ValidarCampoObrigatorio(oneDriveConfig.ClientId, nameof(oneDriveConfig.ClientId));
            ValidarCampoObrigatorio(oneDriveConfig.ClientSecret, nameof(oneDriveConfig.ClientSecret));

            return new OneDriveBackupDestino();
        }

        /// <summary>
        /// Método auxiliar para validar se um campo obrigatório não está vazio
        /// </summary>
        /// <param name="valor">Valor a ser validado</param>
        /// <param name="nomeCampo">Nome do campo para mensagem de erro</param>
        /// <exception cref="InvalidOperationException">Quando o campo está vazio ou nulo</exception>
        private static void ValidarCampoObrigatorio(string valor, string nomeCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException($"O campo '{nomeCampo}' é obrigatório e não pode estar vazio");
        }
    }
}
