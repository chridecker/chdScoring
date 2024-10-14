using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace chdScoring.App.WPF.Hosting
{
    public class WPFLifetime : IHostLifetime, IDisposable
    {
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
        private readonly IHostEnvironment _environment;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;

        public WPFLifetime(ILogger<WPFLifetime> logger,
            IHostEnvironment environment,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            this._logger = logger;
            this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this._applicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            this._applicationStartedRegistration = this._applicationLifetime.ApplicationStarted.Register(state =>
            {
                ((WPFLifetime)state).OnApplicationStarted();
            },
            this);
            this._applicationStoppingRegistration = this._applicationLifetime.ApplicationStopping.Register(state =>
            {
                ((WPFLifetime)state).OnApplicationStopping();
            },
            this);
            return Task.CompletedTask;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
        private void OnApplicationStarted()
        {
            this._logger.LogInformation("Hosting environment: {envName}", this._environment.EnvironmentName);
            this._logger.LogInformation("Content root path: {contentRoot}", this._environment.ContentRootPath);
        }

        private void OnApplicationStopping()
        {
            this._logger.LogInformation("Application is shutting down...");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public void Dispose()
        {
            this._applicationStartedRegistration.Dispose();
            this._applicationStoppingRegistration.Dispose();

        }
    }
}
