using Android;

namespace chdScoring.App.Platforms.Android
{
    public class WifiPermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                var result = new List<(string androidPermission, bool isRuntime)>();
                if (OperatingSystem.IsAndroidVersionAtLeast(33))
                    result.Add((Manifest.Permission.AccessWifiState, true));
                return result.ToArray();
            }
        }
    }
}