using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using BackupAutomaticoCervantes.Models;
using BackupAutomaticoCervantes.DestinoBackup;
using System.Text;
using BackupAutomaticoCervantes.Padrao;

namespace BackupAutomaticoCervantes.Services
{
    /// <summary>
    /// Responsável por executar a rotina de backup para um único parâmetro de backup.
    /// </summary>
    public class BackupManager
    {
        /// <summary>
        /// Executa o backup conforme as configurações definidas em ParametrosBackupModel.
        /// </summary>
        /// <param name="param">Parâmetros de backup contendo conexão, agendamentos e destinos.</param>
        public void ExecuteBackup(ParametrosBackupModel param)
        {
            // Gera nome de arquivo com timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFileName = $"{param.NomebancoBancoDados}_{timestamp}.dump";

            // Diretório temporário para gerar o backup
            string tempDir = Path.Combine(Path.GetTempPath(), "BackupAutomaticoCervantes");
            Directory.CreateDirectory(tempDir);

            string backupFilePath = Path.Combine(tempDir, backupFileName);

            try
            {
                // Executa pg_dump para gerar o arquivo de backup
                RunPgDump(param, backupFilePath);

                // Envia o arquivo de backup para cada destino configurado
                foreach (var destino in param.Destinos)
                {
                    SaveToDestination(destino, backupFilePath);
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode adicionar logging ou rethrow conforme necessidade
                throw new ApplicationException($"Erro ao executar backup para {param.NomebancoBancoDados}: {ex.Message}", ex);
            }
            finally
            {
                // Limpeza: remove arquivo temporário após distribuição
                try
                {
                    if (File.Exists(backupFilePath))
                        File.Delete(backupFilePath);
                }
                catch
                {
                    // Ignora falhas na limpeza
                }
            }
        }

        /// <summary>
        /// Executa o processo pg_dump com base nos parâmetros de conexão.
        /// </summary>
        private void RunPgDump(ParametrosBackupModel param, string outputFile)
        {
            Environment.SetEnvironmentVariable("PGPASSWORD", param.SenhaUsuario);

            var stringTabelasIgnoradas = new StringBuilder();

            if (param.ListaTabelasIgnoradas != null)
            {
                foreach (string tabela in param.ListaTabelasIgnoradas)
                    stringTabelasIgnoradas.Append(" --exclude-table-data=" + tabela);
            }

            string backupString = stringTabelasIgnoradas + $@"
                --exclude-table-data=audit.logged_actions 
                --format custom 
                --blobs 
                --verbose
                --file {outputFile} 
                --host {param.Servidor} 
                --username {param.UsuarioBancoDados}
                --port {param.Porta} 
                --verbose 
                {param.NomebancoBancoDados}";

            ClsStatica.EscreverLog("Fazendo backup");

            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = param.CaminhoPgDump,
                    Arguments = backupString,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;

                if (!File.Exists(process.StartInfo.FileName))
                    throw new Exception(String.Format("Diretório não existe: {0}", process.StartInfo.FileName.ToString()));

                process.Start();

                // To avoid deadlocks, always read the output stream first and then wait. https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.standarderror?view=netcore-3.1
                string output = process.StandardError.ReadToEnd();

                process.WaitForExit();

                var result = new StringBuilder();
                result.AppendLine($"Process returned exit code {process.ExitCode}.");
                result.AppendLine($"{process.StartInfo.FileName} {process.StartInfo.Arguments}");

                if (process.ExitCode != 0)
                {
                    //Vitor: Somente concatena o Output em caso de erro, para que o LOG não fique muito grande.
                    result.AppendLine(output);
                    ClsStatica.EscreverLog(result.ToString());

                    throw new Exception("Erro ao fazer backup", new Exception(result.ToString()));
                }
                else
                    ClsStatica.EscreverLog(result.ToString());
            }
        }

        /// <summary>
        /// Envia o arquivo de backup para o destino configurado.
        /// </summary>
        private void SaveToDestination(IDestinoConfig destino, string filePath)
        {
            switch (destino.Tipo)
            {   
                case DestinoTipo.OneDrive:    // OneDrive
                    // TODO: implementar lógica de upload para OneDrive
                    // Exemplo: OneDriveClient.Upload(filePath, od.PastaDestino, od.CredenciaisJsonPath);
                    break;

                case DestinoTipo.GoogleDrive: // Google Drive
                    // TODO: implementar lógica de upload para Google Drive
                    // Exemplo: GoogleDriveClient.Upload(filePath, gd.CredenciaisJsonPath);
                    break;

                case DestinoTipo.Ftp:        // FTP
                    // TODO: implementar lógica de upload FTP
                    // Exemplo: FtpClient.Upload(ftp.Host, ftp.Porta, ftp.Usuario, ftp.Senha, filePath);
                    break;

                case DestinoTipo.S3:          // AWS S3
                    // TODO: implementar lógica de upload S3
                    // Exemplo: S3Client.Upload(filePath, s3.BucketName, s3.CredenciaisJsonPath);
                    break;

                default:
                    throw new NotSupportedException($"Destino não suportado: {destino.GetType().Name}");
            }
        }
    }
}
