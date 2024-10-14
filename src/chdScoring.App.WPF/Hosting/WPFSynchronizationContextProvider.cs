using System.Windows;
using System.Windows.Threading;

namespace chdScoring.App.WPF.Hosting
{
    public interface IGuiContext
    {
        void Invoke(Action action);

        TResult Invoke<TResult>(Func<TResult> func);

        Task<TResult> InvokeAsync<TResult>(Func<TResult> func);

    }

    public interface IWPFSynchronizationContextProvider
    {
        Dispatcher Dispatcher { get; set;}
    }

    public class WPFSynchronizationContextProvider : IWPFSynchronizationContextProvider, IGuiContext
    {
        public Dispatcher Dispatcher { get; set; }

        public void Invoke(Action action) => Application.Current.Dispatcher.Invoke(action);

        public TResult Invoke<TResult>(Func<TResult> func) => Application.Current.Dispatcher.Invoke(func);

        public async Task<TResult> InvokeAsync<TResult>(Func<TResult> func) => await Application.Current.Dispatcher.InvokeAsync(func);

    }
}
