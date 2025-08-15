// Program.cs
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MigracaoPostgreSQL.Models;
using MigracaoPostgreSQL.Repositories;
using MigracaoPostgreSQL.Services;
using MigracaoPostgreSQL.Utils;

namespace MigracaoPostgreSQL
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger("migracao_log.txt");
            
            try
            {
                logger.LogInfo("=== INÍCIO DA MIGRAÇÃO DE DADOS ===");
                
                var migrationService = new MigrationService(logger);
                await migrationService.ExecuteMigrationAsync();
                
                logger.LogInfo("=== MIGRAÇÃO CONCLUÍDA COM SUCESSO ===");
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro crítico na aplicação: {ex.Message}", ex);
            }
            finally
            {
                logger.Dispose();
                Console.WriteLine("Migração finalizada. Pressione qualquer tecla para sair...");
                Console.ReadKey();
            }
        }
    }
}

namespace MigracaoPostgreSQL.Repositories
{
    

    
}

namespace MigracaoPostgreSQL.Repositories
{
    
}

namespace MigracaoPostgreSQL.Repositories
{
    
}

namespace MigracaoPostgreSQL.Services
{
   
}

namespace MigracaoPostgreSQL.Services
{
   

    
}

namespace MigracaoPostgreSQL.Services
{
    
}

namespace MigracaoPostgreSQL.Utils
{
    
}

namespace MigracaoPostgreSQL.Utils
{
    
}