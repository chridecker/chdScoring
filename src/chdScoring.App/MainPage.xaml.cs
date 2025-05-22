#if ANDROID
using AndroidX.Activity;
using chdScoring.App.Platforms.Android;
#endif
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;

namespace chdScoring.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            try
            {
                this.blazorWebView.BlazorWebViewInitialized += this.BlazorWebViewInitialized;
            }
            catch (Exception ex)
            {
                // do something if error appears
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DeviceDisplay.KeepScreenOn = true;
            await this.CheckPermissions();
        }

        private async Task CheckPermissions()
        {
#if ANDROID
            PermissionStatus statusNotification = await Permissions.RequestAsync<NotificationPermission>();
            PermissionStatus statusWifi = await Permissions.RequestAsync<WifiPermission>();
#endif
        }
        private  void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
#if ANDROID

            try
            {
                if (e.WebView.Context?.GetActivity() is not ComponentActivity activity)
                {
                    throw new InvalidOperationException($"The permission-managing WebChromeClient requires that the current activity be a '{nameof(ComponentActivity)}'.");
                }

                e.WebView.Settings.JavaScriptEnabled = true;
                e.WebView.Settings.AllowFileAccess = true;
                e.WebView.Settings.MediaPlaybackRequiresUserGesture = false;
                var webChromeClient = new PermissionManagingBlazorWebChromeClient(e.WebView.WebChromeClient!, activity);
                e.WebView.SetWebChromeClient(webChromeClient);
            }
            catch (Exception)
            {
                // do something if error appears
            }
#endif

        }
    }
}