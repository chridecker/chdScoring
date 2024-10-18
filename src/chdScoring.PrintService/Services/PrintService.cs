using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System.Drawing.Printing;

namespace chdScoring.PrintService.Services
{
    public class PrintService : BackgroundService
    {
        private readonly IPrintCache _printCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

        public PrintService(IPrintCache printCache, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            this._printCache = printCache;
            this._serviceProvider = serviceProvider;
            this._loggerFactory = loggerFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (!Directory.Exists("Pdf"))
            {
                Directory.CreateDirectory("Pdf");
            }

            var fw = new FileSystemWatcher("Pdf")
            {
                EnableRaisingEvents = true,
                Filter = "*.pdf",
                IncludeSubdirectories = false,
            };
            fw.Created += this.Fw_Changed;
            fw.Changed += this.Fw_Changed;

            return base.StartAsync(cancellationToken);
        }

        private async void Fw_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Created)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                var file = new FileInfo(e.FullPath);
                if (file.Exists)
                {
                    this.PrintFile(file);
                }
            }
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(this._printCache.Printer))
                    {
                        await this.HandleCache(stoppingToken);
                    }
                }
                catch (Exception ex)
                {

                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task HandleCache(CancellationToken cancellationToken)
        {
            while (this._printCache.TryTake(out var dto, cancellationToken))
            {
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                });
                using var client = new HttpClient()
                {
                    BaseAddress = new Uri(dto.Url)
                };
                var html = await client.GetStringAsync("");


                var page = await browser.NewPageAsync(new()
                {
                    Content = html
                });
                await page.PdfAsync(new PagePdfOptions()
                {
                    Format = "A4",
                    Landscape = false,
                    Path = $"Pdf/test.pdf"
                });
                await page.CloseAsync();
            }
        }

        private async Task HandleWithComponent()
        {
            await using var renderer = new HtmlRenderer(this._serviceProvider, this._loggerFactory);
           var html = await  renderer.Dispatcher.InvokeAsync(async () =>
            {
                var dict = new Dictionary<string, object>()
                {

                };

                var param = ParameterView.FromDictionary(dict);
                var output = await renderer.RenderComponentAsync<Comonent>(param);
                return output.ToHtmlString();
            });
        }

        private void PrintFile(FileInfo info)
        {
            var pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = this._printCache.Printer;
            pd.PrinterSettings.PrintFileName = info.FullName;
            pd.DocumentName = info.FullName;
            pd.Print();
            info.Delete();

        }
    }
}
