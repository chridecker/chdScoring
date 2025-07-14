using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Activity;
using AndroidX.Core.View;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Services;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Enums;
using System.Text.Json;

namespace chdScoring.App
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density | ConfigChanges.Keyboard | ConfigChanges.Keyboard | ConfigChanges.Navigation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MauiAppCompatActivity
    {
        private readonly INotificationManagerService _notificationManagerService;
        private readonly IAppInfoService _appInfoService;
        private readonly IKeyHandler _keyHandler;
        private readonly IToastHandler _toastHandler;

        public MainActivity()
        {
            this._notificationManagerService = IPlatformApplication.Current.Services.GetService<INotificationManagerService>();
            this._appInfoService = IPlatformApplication.Current.Services.GetService<IAppInfoService>();
            this._keyHandler = IPlatformApplication.Current.Services.GetService<IKeyHandler>();
            this._toastHandler = IPlatformApplication.Current.Services.GetService<IToastHandler>();
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.CreateNotificationFromIntent(Intent);

            this.OnBackPressedDispatcher.AddCallback(this, new BackPress(this._appInfoService));

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
            this.CreateNotificationFromIntent(intent);
        }


        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            if ((e.Source & InputSourceType.Gamepad) == InputSourceType.Gamepad)
            {

            }
            return base.OnKeyDown(keyCode, e);
        }
        public override bool OnGenericMotionEvent(MotionEvent e)
        {
            if ((e.Source & InputSourceType.Joystick) == InputSourceType.Joystick
                && e.Action == MotionEventActions.Move)
            {
                // l r
                var x = __getCenteredAxis(e, e.Device, Axis.X);
                // o u
                var y = __getCenteredAxis(e, e.Device, Axis.Y); // 0,003921509




                return true;
            }
            return base.OnGenericMotionEvent(e);

            float __getCenteredAxis(MotionEvent e, InputDevice device, Axis axis)
            {
                InputDevice.MotionRange range = device.GetMotionRange(axis, e.Source);
                if (range != null)
                {
                    return e.GetAxisValue(axis);
                }
                return 0;
            }
            EJoystickMotionDirection __getDirection(float x, float y)
            {
                const float deadZone = 0.25f; // kleiner Bereich, um ungewollte Bewegungen zu ignorieren

                if (Math.Abs(x) < deadZone && Math.Abs(y) < deadZone)
                {
                    return EJoystickMotionDirection.Center;
                }

                if (Math.Abs(x) > Math.Abs(y))
                {
                    return x > 0 ? EJoystickMotionDirection.Right : EJoystickMotionDirection.Left;
                }
                else
                {
                    return y > 0 ? EJoystickMotionDirection.Down : EJoystickMotionDirection.Up;
                }
            }
        }



        private EKeyInput GetInput(Keycode keyCode, KeyEvent? e) => (keyCode, e.Action) switch
        {
            (Keycode.ButtonA, KeyEventActions.Down) => EKeyInput.A,
            (Keycode.ButtonB, KeyEventActions.Down) => EKeyInput.B,
            (Keycode.ButtonX, KeyEventActions.Down) => EKeyInput.X,
            (Keycode.ButtonY, KeyEventActions.Down) => EKeyInput.Y,
            (Keycode.Menu, KeyEventActions.Down) => EKeyInput.Menu,
            _ => EKeyInput.None
        };


        private void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                //var reply = this.GetReply(intent);

                var id = intent.GetIntExtra(Platforms.Android.NotificationManagerService.IdKey, 0);
                var title = intent.GetStringExtra(Platforms.Android.NotificationManagerService.TitleKey);
                var message = intent.GetStringExtra(Platforms.Android.NotificationManagerService.MessageKey);
                var cancel = intent.GetBooleanExtra(Platforms.Android.NotificationManagerService.CancelKey, false);
                object intentData = null;

                if (intent.HasExtra(Platforms.Android.NotificationManagerService.DataKey))
                {

                    string data = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataKey);
                    string type = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataTypeKey);

                    var t = Type.GetType(type);
                    intentData = JsonSerializer.Deserialize(data, t);
                }
                this._notificationManagerService.ReceiveNotification(new NotificationEventArgs(id, title, message, intentData, cancel));
            }
        }

        //private string GetReply(Intent intent)
        //{
        //    var input = RemoteInput.GetResultsFromIntent(intent);
        //    if (input is not null)
        //    {
        //        return input.GetCharSequence("key_text_reply", "");
        //    }
        //    return string.Empty;
        //}

        class BackPress : OnBackPressedCallback
        {
            private readonly IAppInfoService _appInfoService;

            public BackPress(IAppInfoService appInfoService) : base(true)
            {
                this._appInfoService = appInfoService;
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

                    this._appInfoService.BackButtonPressed?.Invoke(this, false);
                }
            }
        }
    }


}