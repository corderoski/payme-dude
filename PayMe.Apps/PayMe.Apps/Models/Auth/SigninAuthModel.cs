
namespace PayMe.Apps.Models.Auth
{
    public class SigninAuthModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string DeviceChecksum { get; set; }
    }
}
