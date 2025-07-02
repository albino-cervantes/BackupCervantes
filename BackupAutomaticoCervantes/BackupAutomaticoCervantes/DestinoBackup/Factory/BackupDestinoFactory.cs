using BackupAutomaticoCervantes.DestinoBackup.Amazon;
using BackupAutomaticoCervantes.DestinoBackup.Ftp;
using BackupAutomaticoCervantes.DestinoBackup.GoogleDrive;
using BackupAutomaticoCervantes.DestinoBackup.OneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.DestinoBackup.Factory
{
    /// <summary>
    /// Factory responsável por criar instâncias de destinos de backup
    /// Implementa o padrão Factory Method para encapsular a lógica de criação
    /// </summary>
    public class BackupDestinoFactory : IBackupDestinoFactory
    {
        public IBackupDestino CriarDestino(DestinoConfig configuracao)
        {
            if (configuracao == null)
                throw new ArgumentNullException(nameof(configuracao), "A configuração do destino não pode ser nula");

            // Substituir o switch expression por um switch statement para compatibilidade com C# 7.3
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
                    throw new NotSupportedException($"Tipo de destino '{configuracao.Tipo}' não é suportado");
            }
        }

        private IBackupDestino CriarDestinoFtp(DestinoConfig configuracao)
        {
            if (configuracao.Ftp == null)
                throw new InvalidOperationException("Configuração FTP não pode ser nula para destino do tipo FTP");

            ValidarCampoObrigatorio(configuracao.Ftp.Url, nameof(configuracao.Ftp.Url));
            ValidarCampoObrigatorio(configuracao.Ftp.Usuario, nameof(configuracao.Ftp.Usuario));
            ValidarCampoObrigatorio(configuracao.Ftp.Senha, nameof(configuracao.Ftp.Senha));

            return new FtpBackupDestino();
        }

        private IBackupDestino CriarDestinoGoogleDrive(DestinoConfig configuracao)
        {
            if (configuracao.GoogleDrive == null)
                throw new InvalidOperationException("Configuração Google Drive não pode ser nula para destino do tipo Google Drive");

            return new GoogleDriveBackupDestino(configuracao.Id);
        }

        private IBackupDestino CriarDestinoS3(DestinoConfig configuracao)
        {
            if (configuracao.S3 == null)
                throw new InvalidOperationException("Configuração S3 não pode ser nula para destino do tipo S3");

            ValidarCampoObrigatorio(configuracao.S3.AccessKey, nameof(configuracao.S3.AccessKey));
            ValidarCampoObrigatorio(configuracao.S3.SecretKey, nameof(configuracao.S3.SecretKey));
            ValidarCampoObrigatorio(configuracao.S3.BucketName, nameof(configuracao.S3.BucketName));
            ValidarCampoObrigatorio(configuracao.S3.Region, nameof(configuracao.S3.Region));

            return new AmazonS3BackupDestino();
        }

        private IBackupDestino CriarDestinoOneDrive(DestinoConfig configuracao)
        {
            if (configuracao.OneDrive == null)
                throw new InvalidOperationException("Configuração OneDrive não pode ser nula para destino do tipo OneDrive");

            ValidarCampoObrigatorio(configuracao.OneDrive.TenantId, nameof(configuracao.OneDrive.TenantId));
            ValidarCampoObrigatorio(configuracao.OneDrive.ClientId, nameof(configuracao.OneDrive.ClientId));
            ValidarCampoObrigatorio(configuracao.OneDrive.ClientSecret, nameof(configuracao.OneDrive.ClientSecret));

            return new OneDriveBackupDestino();
        }

        private static void ValidarCampoObrigatorio(string valor, string nomeCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException($"O campo '{nomeCampo}' é obrigatório e não pode estar vazio");
        }
    }
}
