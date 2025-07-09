using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Services
{
    public class ToastHandler : ToastService, IToastHandler
    {
        public new void ShowInfo(string message, Action<ToastSettings>? settings = null) => Toast.Make(message).Show().Wait(TimeSpan.FromSeconds(5));
    }

    public interface IToastHandler : IToastService
    {

    }
}
