using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDriveType
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var credPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gdrive-sa.json");
            var parentFolderId = "13Ss3ABiJVAwVNuitndeeV5LN1yh4VDNX";

            GoogleCredential cred;
            using (var s = new FileStream(credPath, FileMode.Open))
                cred = GoogleCredential.FromStream(s).CreateScoped(new[] { DriveService.Scope.Drive });

            var svc = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = cred,
                ApplicationName = "CheckDriveType"
            });

            try
            {
                var req = svc.Files.Get(parentFolderId);
                req.Fields = "id,name,parents,driveId,owners,permissions";
                req.SupportsAllDrives = true;
                var file = req.Execute();

                Console.WriteLine($"Name: {file.Name}");
                Console.WriteLine($"Id: {file.Id}");
                Console.WriteLine($"DriveId: {file.DriveId ?? "(null)"}");
                Console.WriteLine($"Owners count: {file.Owners?.Count ?? 0}");
                if (file.Permissions != null)
                {
                    foreach (var p in file.Permissions)
                        Console.WriteLine($"Permission: id={p.Id}, type={p.Type}, role={p.Role}, email={p.EmailAddress}");
                }

                if (!string.IsNullOrWhiteSpace(file.DriveId))
                    Console.WriteLine("=> Este folder está em um Shared Drive (Drive compartilhado).");
                else
                    Console.WriteLine("=> Este folder pertence ao My Drive (não Shared Drive).");
            }
            catch (Google.GoogleApiException gae)
            {
                Console.WriteLine("Google API error: " + gae.Message);
                Console.WriteLine("Detalhes: " + gae.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex);
            }

        }
    }
}
