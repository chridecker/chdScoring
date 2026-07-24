using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.Main.WebServer.Services
{
    public class ImportExecuteService : BackgroundService
    {
        private readonly IApiLogger _apiLogger;
        private readonly IDataImportService _dataImportService;

        private string _folder;

        private BlockingCollection<ImportRoundScoreDto> _importCollection = [];
        private Task _executer;

        public ImportExecuteService(IApiLogger apiLogger, IDataImportService dataImportService)
        {
            _apiLogger = apiLogger;
            _dataImportService = dataImportService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this._folder = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Import);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            this._executer = this._executeImport(cancellationToken);

            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.HandleImportFolder(stoppingToken);
                }
                catch (Exception ex)
                {
                    await this._apiLogger.Log(ex.Message);
                }
            }
        }

        private Task _executeImport(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            foreach (var importDto in _importCollection)
            {
                await this._dataImportService.ImportAsync(importDto, cancellationToken);
            }
        }, cancellationToken);


        private async Task HandleImportFolder(CancellationToken cancellationToken)
        {
            foreach (var file in Directory.GetFiles(this._folder, "*.json"))
            {
                var info = new FileInfo(file);

                this._importCollection.TryAdd(new ImportRoundScoreDto()
                {

                });
            }
        }
    }
}
