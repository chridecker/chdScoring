using chdScoring.App.Interfaces;

namespace chdScoring.App.Helper
{
    public class DialogHelper : IDialogHelper
    {
         public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
            => await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons));

  
        public async Task DisplayAlert(string title , string message , string cancel)
            => await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(title, message, cancel));

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
            => await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel));
    }
   
}
