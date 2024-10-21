using Blazored.Toast;
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using CommunityToolkit.Maui.Alerts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Android
{
    public class AndroidToastService : ToastService
    {
        public event Action<ToastLevel, RenderFragment, Action<ToastSettings>> OnShow;
        public event Action OnClearAll;
        public event Action<ToastLevel> OnClearToasts;
        public event Action OnClearCustomToasts;
        public event Action<Type, ToastParameters, Action<ToastSettings>> OnShowComponent;
        public event Action OnClearQueue;
        public event Action<ToastLevel> OnClearQueueToasts;

        public new void ShowInfo(string message, Action<ToastSettings> settings = null)
        {
            var toast = Toast.Make(message);
            toast.Show().Wait();
        }
    }
}
