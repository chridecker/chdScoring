using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.WebServer.Services
{
    public class PrintExecuteService : BackgroundService
    {
        private readonly IPrintCache _printCache;
        private readonly IApiLogger _logger;
        private readonly IDatabaseConfiguration _databaseConfiguration;
        private string _folder;


        public PrintExecuteService(IPrintCache printCache, IApiLogger logger, IDatabaseConfiguration databaseConfiguration)
        {
            this._printCache = printCache;
            this._logger = logger;
            this._databaseConfiguration = databaseConfiguration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this._folder = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Folder);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
            foreach (var db in this._databaseConfiguration.GetConnections())
            {
                if (!Directory.Exists(Path.Combine(_folder, FolderConstants.Printed, db.Name)))
                {
                    Directory.CreateDirectory(Path.Combine(_folder, FolderConstants.Printed, db.Name));
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
                    await this.HandleCache(stoppingToken);

                    await this.HandlePrintFolder(stoppingToken);

                    if (!string.IsNullOrWhiteSpace(this._printCache.Printer))
                    {
                        await this.HandlePrintQueue(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Log(ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task HandleCache(CancellationToken cancellationToken)
        {
            while (this._printCache.TryTake(out CreatePdfDto dto, cancellationToken))
            {
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                });

                var page = await browser.NewPageAsync();
                var respoonse = await page.GotoAsync(dto.Url);
                await Task.Delay(500, cancellationToken);
                await page.PdfAsync(new PagePdfOptions()
                {
                    Format = "A4",
                    Landscape = dto.Landscape,
                    Path = $"{FolderConstants.Folder}/{dto.Name}"
                });
                await page.CloseAsync();
            }
        }

        private async Task HandlePrintQueue(CancellationToken cancellationToken)
        {
            while (this._printCache.TryTake(out FileInfo file, cancellationToken))
            {
                if (file.Exists && this.PrintFileToPrinter(file, this._printCache.Printer))
                {
                    var printed = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Folder, FolderConstants.Printed, this._databaseConfiguration.CurrentConnection, file.Name);
                    file.MoveTo(printed, true);
                }
            }
        }

        private async Task HandlePrintFolder(CancellationToken cancellationToken)
        {
            foreach (var file in Directory.GetFiles(this._folder, "*.pdf"))
            {
                var info = new FileInfo(file);

                if (this._printCache.AutoPrint && info.Exists && info.CreationTime < DateTime.Now.AddSeconds(2))
                {
                    this._printCache.Add(info);
                }
            }
        }

        private bool PrintFileToPrinter(FileInfo info, string printer)
        {
            try
            {
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printer,
                    Copies = (short)1,
                };

                var pageSettings = new PageSettings(printerSettings)
                {
                    Margins = new Margins(0, 0, 0, 0),
                };
                foreach (PaperSize paperSize in printerSettings.PaperSizes)
                {
                    if (paperSize.Kind == PaperKind.A4)
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }

                using (var document = PdfDocument.Load(info.FullName))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this._logger?.Log(ex.Message);
                return false;
            }
        }
    }
}
