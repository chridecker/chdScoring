﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public interface IDialogHelper
    {
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
        Task DisplayAlert(string title, string message, string cancel);
    }
}
