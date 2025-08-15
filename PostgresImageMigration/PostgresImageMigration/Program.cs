using System;
using PostgresImageMigration.Services;
using PostgresImageMigration.Utils;

namespace PostgresImageMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Initialize(); // inicializa arquivo de log
            Logger.Log("=== Iniciando aplicação de migração de imagens ===");

            // Cria e executa o serviço de migração (o serviço faz todo o pipeline)
            try
            {
                using (var migracao = new FotoMigrationService())
                {
                    migracao.MigrarImagens();
                }
                Logger.Log("Processo finalizado com sucesso.");
            }
            catch (Exception ex)
            {
                Logger.Log("Erro irreversível no Program: " + ex.ToString());
            }

            Logger.Log("=== Fim ===");
            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine();
        }
    }
}
