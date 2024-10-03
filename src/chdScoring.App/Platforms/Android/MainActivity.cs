using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using AndroidX.Activity;
using chd.UI.Base.Contracts.Interfaces.Services;

namespace chdScoring.App
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private readonly IAppInfoService _appInfoService;
        public MainActivity()
        {
            this._appInfoService = IPlatformApplication.Current.Services.GetService<IAppInfoService>();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.OnBackPressedDispatcher.AddCallback(this, new BackPress());
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

                    service.BackButtonPressed.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }


}