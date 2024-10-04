using System.Net.Security;
#if ANDROID
using Xamarin.Android.Net;
#endif

namespace chdScoring.App.Auth
{
    public static class HttpsClientHandlerService
    {

        #if ANDROID
        public static HttpMessageHandler GetPlatformMessageHandler() => new AndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Contains("Friedrich Deutsch Metallwerk Ges.m.b.H"))
                    return true;
                return errors == SslPolicyErrors.None;
            }
        };
        #endif

    }
}
