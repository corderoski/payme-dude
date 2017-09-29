using System.Threading.Tasks;

namespace PayMe.Apps.Services
{
    public interface IDeviceConnectionHelper
    {
        /// <summary>
        /// Checks for Internet connection (given a network or WiFi net).
        /// </summary>
        /// <exception cref="Models.DevicePermissionNotLocatedException">In case that required permissions hasn't been granted.</exception>
        /// <returns></returns>
        Task<bool> IsNetworkConnectedAsync();
        Task<bool> HasServerConnectionAsync(string hostServerUrl = "");
    }
}
