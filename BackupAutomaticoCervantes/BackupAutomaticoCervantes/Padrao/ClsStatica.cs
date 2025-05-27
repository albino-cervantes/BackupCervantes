using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAutomaticoCervantes.Padrao
{
    public class ClsStatica
    {
        public static void EscreverLog(string pTexto, string pPath, bool pSuprimirData = false)
        {
            try
            {
                //Tiago: se o diretório ainda não existir
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pPath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(pPath));

                System.IO.StreamWriter stream = new System.IO.StreamWriter(pPath, true);
                stream.WriteLine((pSuprimirData ? "" : DateTime.Now.ToString("dd/MM/yy HH:mm:ss - ")) + pTexto);
                stream.Close();
            }
            catch { }
        }
    }
}
