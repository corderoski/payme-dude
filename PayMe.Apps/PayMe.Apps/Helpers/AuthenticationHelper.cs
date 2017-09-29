using Microsoft.WindowsAzure.MobileServices;

namespace PayMe.Apps.Helpers
{
    internal sealed class AuthenticationHelper
    {
        public static MobileServiceAuthenticationProvider[] Providers { get => new[] { MobileServiceAuthenticationProvider.Facebook }; }
    }
}
