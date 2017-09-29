using System.Threading.Tasks;

namespace PayMe.Apps.Services
{

    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string UserId { get; set; }
    }

    public interface IAuthentication
    {
        Task<AuthenticationResult> Authenticate(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider authenticationProvider);
    }
}
