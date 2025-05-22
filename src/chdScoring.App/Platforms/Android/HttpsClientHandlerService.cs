using System.Net.Security;
#if ANDROID
using Xamarin.Android.Net;
#endif

namespace chdScoring.App.Platforms.Android
{
    public static class HttpsClientHandlerService
    {
        public static HttpMessageHandler GetPlatformMessageHandler() => new AndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>true,
        };

    }
}
