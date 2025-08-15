using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.IO;

namespace BackupAutomaticoCervantes.Padrao
{
    public class ClsStatica
    {
        public static void EscreverLog(string pTexto, string pPath = null, bool pSuprimirData = false)
        {
            try
            {
                if (Path.GetDirectoryName(pPath) == null)
                    pPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs.txt");

                //Tiago: se o diretório ainda não existir
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pPath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pPath));

                System.IO.StreamWriter stream = new System.IO.StreamWriter(pPath, true);
                stream.WriteLine((pSuprimirData ? "" : DateTime.Now.ToString("dd/MM/yy HH:mm:ss - ")) + pTexto);
                stream.Close();
            }
            catch { }
        }

        public static void CriarServico(string pNomeServico, string pDescricao, string pPathExe)
        {
            try
            {
                // Verifica se o serviço já existe
                if (System.ServiceProcess.ServiceController.GetServices().Any(s => s.ServiceName == pNomeServico))
                    return;
                // Cria o serviço
                System.Configuration.Install.ManagedInstallerClass.InstallHelper(new[] { pPathExe });
            }
            catch (Exception ex)
            {
                EscreverLog($"Erro ao criar serviço: {ex.Message}", "erro_servico.log");
            }
        }
    }
}
