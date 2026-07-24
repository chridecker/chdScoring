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
using System.Text.Json;

namespace chdScoring.Main.WebServer.Services
{
    public class ImportExecuteService : BackgroundService
    {
        private readonly IApiLogger _apiLogger;
        private readonly IDataImportService _dataImportService;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private string _folder;

        private BlockingCollection<ImportRoundScoreDto> _importCollection = [];
        private Task _executer;

        public ImportExecuteService(IApiLogger apiLogger, IDataImportService dataImportService, IDatabaseConfiguration databaseConfiguration)
        {
            _apiLogger = apiLogger;
            _dataImportService = dataImportService;
            this._databaseConfiguration = databaseConfiguration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this._folder = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Import);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            foreach (var db in this._databaseConfiguration.GetConnections())
            {
                var imported = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Import, FolderConstants.Imported, db.Name);
                if (!Directory.Exists(imported))
                {
                    Directory.CreateDirectory(imported);
                }
            }

            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.HandleImportFolder(stoppingToken);
                    await this.ExecuteImport(stoppingToken);
                }
                catch (Exception ex)
                {
                    await this._apiLogger.Log(ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ExecuteImport(CancellationToken cancellationToken)
        {
            while (this._importCollection.TryTake(out var importDto, 10, cancellationToken))
            {
                await this._dataImportService.ImportAsync(importDto, cancellationToken);
            }
        }


        private async Task HandleImportFolder(CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(this._folder, "*.json").ToList();
            foreach (var fileName in files)
            {
                var file = new FileInfo(fileName);

                var dto = await this.CreateDtoFromFile(file, cancellationToken);

                this._importCollection.TryAdd(dto);
                var imported = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Import, FolderConstants.Imported, this._databaseConfiguration.CurrentConnection, file.Name);
                file.MoveTo(imported, true);
            }
        }

        private async Task<ImportRoundScoreDto> CreateDtoFromFile(FileInfo file, CancellationToken cancellationToken)
        {
            var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            var scores = await JsonSerializer.DeserializeAsync<decimal[]>(fs, JsonSerializerOptions.Web, cancellationToken);
            await fs.DisposeAsync();

            var dto = new ImportRoundScoreDto
            {
                Judge = 1,
            };
            for (int i = 0; i < scores.Length; i++)
            {
                dto.Scores.Add(new()
                {
                    Figure = i + 1,
                    Value = scores[i]
                });
            }
            var fileName = file.Name.Replace(file.Extension, "");
            if (fileName.StartsWith("R")
                && fileName.Contains("P")
                && fileName.Split("_").Length == 4)
            {
                dto.Round = int.TryParse(fileName.Split("_")[1], out var round) ? round : 0;
                dto.Pilot = int.TryParse(fileName.Split("_")[3], out var pilot) ? pilot : 0;
            }
            return dto;
        }
    }
}
