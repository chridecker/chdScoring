using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;

namespace chdScoring.App.Platforms.Android
{
    [Activity(Process = ":restart", Exported = false,
        Theme = "@android:style/Theme.Translucent.NoTitleBar")]
    public class RestartActivity : Activity
    {
        bool _done;

        protected override void OnResume()
        {
            base.OnResume();
            if (_done)
            {
                return;
            }
            _done = true;

            var pid = Intent!.GetIntExtra("main_pid", -1);
            if (pid > 0)
            {
                Process.KillProcess(pid);
            }

            var launch = PackageManager!.GetLaunchIntentForPackage(PackageName!)!;
            launch.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            StartActivity(launch);

            Finish();
            new Handler(Looper.MainLooper!).PostDelayed(() => Process.KillProcess(Process.MyPid()), 300);
        }
    }
}
