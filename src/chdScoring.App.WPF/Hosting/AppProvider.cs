using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace chdScoring.App.WPF.Hosting
{
    public interface IAppProvider
    {
        Task<T> GetWindowAsync<T>() where T : Window;

        Task<Application> GetMainAppAsync();
    }

    public class AppProvider : IAppProvider, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly IServiceProvider _serviceProvider;
        private readonly IWPFSynchronizationContextProvider _syncContextManager;

        public AppProvider(IServiceProvider serviceProvider, IWPFSynchronizationContextProvider syncContextManager)
        {
            this._serviceProvider = serviceProvider;
            this._syncContextManager = syncContextManager;
        }

        public async Task<T> GetWindowAsync<T>() where T : Window
        {
            await this._semaphore.WaitAsync();
            var form = await this._syncContextManager.Dispatcher.InvokeAsync(() => this._serviceProvider.GetService<T>());
            this._semaphore.Release();
            return form;
        }

        public Task<Application> GetMainAppAsync()
        {
            var applicationContext = this._serviceProvider.GetService<Application>();
            return Task.FromResult(applicationContext);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public void Dispose() => this._semaphore?.Dispose();
    }
}
