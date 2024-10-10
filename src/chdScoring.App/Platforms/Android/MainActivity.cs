using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using AndroidX.Activity;
using AndroidX.Core.View;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace chdScoring.App
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MauiAppCompatActivity
    {
        private readonly IAppInfoService _appInfoService;
        public MainActivity()
        {
            this._appInfoService = IPlatformApplication.Current.Services.GetService<IAppInfoService>();

        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreateNotificationFromIntent(Intent);

            this.OnBackPressedDispatcher.AddCallback(this, new BackPress());

            this.Window?.AddFlags(WindowManagerFlags.Fullscreen);

            WindowCompat.SetDecorFitsSystemWindows(this.Window, false);
            WindowInsetsControllerCompat windowInsetsController = new WindowInsetsControllerCompat(this.Window, this.Window.DecorView);
            // Hide system bars
            windowInsetsController.Hide(WindowInsetsCompat.Type.SystemBars());
            windowInsetsController.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);

            CreateNotificationFromIntent(intent);
        }

        static void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(Platforms.Android.NotificationManagerService.TitleKey);
                string message = intent.GetStringExtra(Platforms.Android.NotificationManagerService.MessageKey);
                string type = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataTypeKey);
                string data = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataKey);
                var cancel = intent.GetBooleanExtra(Platforms.Android.NotificationManagerService.CancelKey, false);


                object intentData = null;

                if (!string.IsNullOrEmpty(type) && Type.GetType(type) is not null && !string.IsNullOrEmpty(data))
                {
                    var t = Type.GetType(type);
                    intentData = JsonSerializer.Deserialize(data, t);
                }

                var service = IPlatformApplication.Current.Services.GetService<INotificationManagerService>();
                service.ReceiveNotification(title, message, intentData, cancel);
            }
        }

        class BackPress : OnBackPressedCallback
        {
            public BackPress() : base(true)
            {
            }

            public override void HandleOnBackPressed()
            {
                var navigation = Microsoft.Maui.Controls.Application.Current?.MainPage?.Navigation;
                if (navigation is not null && navigation.ModalStack.Count > 0)
                {
                    Task.Run(navigation.PopModalAsync);
                }
                else
                {
                    var service = IPlatformApplication.Current.Services.GetService<IAppInfoService>();

                    service.BackButtonPressed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }


}