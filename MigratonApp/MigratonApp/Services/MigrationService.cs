using MigrationApp.Utils;
using System;
using System.Collections.Generic;

namespace MigrationApp.Services
{
    public class MigrationService
    {
        private readonly Logger _logger;
        private readonly List<IOriginRepository> _repositories;
        private readonly DestinationService _destinationService;

        public MigrationService(Logger logger)
        {
            _logger = logger;
            _destinationService = new DestinationService(_logger);
            _repositories = new List<IOriginRepository>
            {
                new Repositories.AutopecasRepository(_logger),
                new Repositories.ConstrucaoRepository(_logger),
                new Repositories.EansRepository(_logger),
                new Repositories.PetshopRepository(_logger)
            };
        }

        public void ExecuteMigration()
        {
            foreach (var repo in _repositories)
            {
                _logger.Info($"Iniciando leitura do repositório: {repo.Name}");

                foreach (var batch in repo.GetBatches(500))
                {
                    try
                    {
                        _destinationService.ProcessBatch(batch);
                        _logger.Info($"Lote com {batch.Count} registros processado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Erro ao processar lote de origem {repo.Name}", ex);
                    }
                }
            }
        }
    }
}