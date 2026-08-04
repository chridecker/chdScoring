using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Services
{
    public class ImportExecuteService : BackgroundService
    {
        private readonly IApiLogger _apiLogger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private readonly IPrintService _printService;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private string _importFolder;

        private BlockingCollection<ImportRoundScoreDto> _importCollection = [];
        private Task _executer;

        public ImportExecuteService(IApiLogger apiLogger, IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration, IPrintService printService,
            IOptionsMonitor<AppSettings> appSettings)
        {
            _apiLogger = apiLogger;
            this._serviceProvider = serviceProvider;
            this._databaseConfiguration = databaseConfiguration;
            this._printService = printService;
            _appSettings = appSettings;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this._importFolder = Path.Combine(this._appSettings.CurrentValue.ImportDirectory);
            var jsonResult = Path.Combine(_importFolder, FolderConstants.JsonResult);
            var bin = Path.Combine(_importFolder, FolderConstants.Bin);
            var json = Path.Combine(_importFolder, FolderConstants.Json);

            this.EnsureFoldersCreates(this._importFolder);
            this.EnsureFoldersCreates(jsonResult);
            this.EnsureFoldersCreates(bin);
            this.EnsureFoldersCreates(json);

            foreach (var db in this._databaseConfiguration.GetConnections())
            {
                var imported = Path.Combine(jsonResult, db.Name);
                this.EnsureFoldersCreates(imported);
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
                try
                {
                    using var scope = this._serviceProvider.CreateAsyncScope();
                    var dataImportService = scope.ServiceProvider.GetService<IDataImportService>();
                    await dataImportService.ImportAsync(importDto, cancellationToken);

                    var url = $"http://localhost/print_durchgang.php?teilnehmer={importDto.Pilot}&round={importDto.Round}";
                    await this._printService.PrintToPdfAsync(new Contracts.Dtos.CreatePdfDto()
                    {
                        Url = url,
                        Name = $"R_{importDto.Round}_P_{importDto.Pilot}.pdf",
                        Landscape = true,
                    }, cancellationToken);
                }
                catch (Exception ex)
                {
                    await this._apiLogger.Log(ex.Message);
                }
            }
        }


        private async Task HandleImportFolder(CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(Path.Combine(this._importFolder, FolderConstants.JsonResult), "*.json").ToList();
            foreach (var fileName in files)
            {
                var file = new FileInfo(fileName);

                var dto = await this.CreateDtoFromFile(file, cancellationToken);
                this._importCollection.TryAdd(dto);
                var imported = Path.Combine(this._importFolder, FolderConstants.JsonResult, this._databaseConfiguration.CurrentConnection, file.Name);
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

        private void EnsureFoldersCreates(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
