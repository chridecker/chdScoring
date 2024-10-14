using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace chdScoring.App.WPF.Hosting
{
    public class WPFHostingService<TApp> : IHostedService where TApp : Application, IInitComponents
    {
        private CancellationTokenRegistration _applicationStoppingRegistration;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IAppProvider _appProvider;
        private readonly IGuiContext _guiContext;
        private readonly IWPFSynchronizationContextProvider _wPFSynchronizationContextProvider;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IServiceProvider _serviceProvider;
        private volatile bool _isShuttingDown = false;

        public WPFHostingService(IHostApplicationLifetime hostApplicationLifetime,
            IAppProvider appProvider,
            IGuiContext guiContext,
            IWPFSynchronizationContextProvider wPFSynchronizationContextProvider,
            IHostEnvironment hostEnvironment,
            IServiceProvider serviceProvider)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._appProvider = appProvider;
            this._guiContext = guiContext;
            this._wPFSynchronizationContextProvider = wPFSynchronizationContextProvider;
            this._hostEnvironment = hostEnvironment;
            this._serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._applicationStoppingRegistration = this._hostApplicationLifetime.ApplicationStopping.Register(state =>
                {
                    ((WPFHostingService<TApp>)state).OnApplicationStopping();
                }, this);

            Thread thread = new(this.StartUiThread);
            thread.Name = "WPF UI Thread";
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void StartUiThread()
        {
            var app = this._serviceProvider.GetRequiredService<TApp>();
            app.Exit += this.App_Exit;
            this._wPFSynchronizationContextProvider.Dispatcher = app.Dispatcher;
            app.DispatcherUnhandledException += this._app_DispatcherUnhandledException;
            app.InitializeComponent();
            app.Run();
        }

        private async void _app_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var app = await this._appProvider.GetMainAppAsync();
            if (app.MainWindow is MainWindow mainWindow)
            {
                var message = "Ein Fehler ist aufgetreten! Möchten sie die Anwendung neustarten?";
#if DEBUG
                message = e.Exception.ToString();
#endif
                if (this._hostEnvironment.IsDevelopment())
                {
                    message = e.Exception.ToString();
                }
                var res = MessageBox.Show(message, "Fehler", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                switch (res)
                {
                    case MessageBoxResult.Yes:
                        mainWindow.Reload();
                        break;
                    case MessageBoxResult.No:
                        this._guiContext.Invoke(() =>
                        {
                            app.Shutdown();
                        });
                        break;
                    default:
                        break;
                }
            }
            e.Handled = true;
        }

        private void App_Exit(object sender, ExitEventArgs e) => this._hostApplicationLifetime.StopApplication();

        private async void OnApplicationStopping()
        {
            var app = await this._appProvider.GetMainAppAsync();
            this._guiContext.Invoke(() =>
            {
                app.Shutdown();
            });

        }
    }
}
