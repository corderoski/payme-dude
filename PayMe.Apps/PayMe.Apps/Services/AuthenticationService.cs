using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using PayMe.Apps.Data;
using PayMe.Apps.Helpers;
using PayMe.Apps.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PayMe.Apps.Services.AuthenticationService))]
namespace PayMe.Apps.Services
{

    public interface IAuthenticationService
    {
        /// <summary>
        /// Indicates the initialization of the Authentication process.
        /// </summary>
        event EventHandler RegistrationStarted;
        /// <summary>
        /// Indicates the completition of the Authentication and his success or failure.
        /// </summary>
        event EventHandler<DataStoreSyncCode> RegistrationCompleted;
        Task DoAuthenticationAsync();
    }

    internal class AuthenticationService : IAuthenticationService
    {

        public event EventHandler RegistrationStarted;
        public event EventHandler<DataStoreSyncCode> RegistrationCompleted;

        public AuthenticationService()
        {
            mobileServiceAuthenticationProvider = MobileServiceAuthenticationProvider.Facebook;
        }

        public async Task DoAuthenticationAsync()
        {
            if (App.Authenticator != null)
            {
                var authenticationResult = await App.Authenticator.Authenticate(mobileServiceAuthenticationProvider);
                if (authenticationResult.Success)
                {
                    PmdAppSetting.UserProviderAuthentication = JsonConvert.SerializeObject(dataStore.CurrentClient.CurrentUser);
                    PmdAppSetting.IsProviderAuthenticated = true;
                }

                /*
                 *  - Sync with the server
                 *  - Register User+Provider
                 */
                if (PmdAppSetting.IsProviderAuthenticated && string.IsNullOrEmpty(PmdAppSetting.RegistrationId))
                {
                    // Step 1
                    RegistrationStarted?.Invoke(this, new EventArgs());

                    var deviceId = GetDeviceRegistrationId();
                    var userDeviceRegistrationModel = new
                    {
                        Email = "mobile@paymedude.net",
                        DeviceId = deviceId,
                        ProviderUserId = authenticationResult.UserId,
                        Provider = mobileServiceAuthenticationProvider.ToString(),
                        Idiom = Plugin.DeviceInfo.CrossDeviceInfo.Current.Idiom.ToString(),
                        Model = Plugin.DeviceInfo.CrossDeviceInfo.Current.Model,
                        Platform = Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform.ToString(),
                        Version = Plugin.DeviceInfo.CrossDeviceInfo.Current.Version,
                    };

                    try
                    {
                        var registrationContent = Newtonsoft.Json.Linq.JToken.FromObject(userDeviceRegistrationModel);
                        var postResult = await dataStore.CurrentClient.InvokeApiAsync("/api/users", registrationContent);

                        var responseModel = postResult.ToObject<Models.Auth.AuthResult>();
                        if (responseModel.Succeeded)
                        {
                            // Step 2
                            PmdAppSetting.RegistrationId = responseModel.UserId;

                            await dataStore.EntitiesRegistrationCheckAsync();
                            var resultCode = await dataStore.SyncAsync();
                            RegistrationCompleted?.Invoke(this, resultCode);
                        }
                    }
                    catch (Exception)
                    {
                        RegistrationCompleted?.Invoke(this, DataStoreSyncCode.ErrorInServer);
                    }
                }

            }
        }

        string GetDeviceRegistrationId()
        {
            var registeredDeviceId = PmdAppSetting.DeviceIdentifier;
            if (string.IsNullOrEmpty(registeredDeviceId))
            {
                var newDeviceId = Plugin.DeviceInfo.CrossDeviceInfo.Current.GenerateAppId(false, "pmd", PmdAppSetting.Version.Replace(".", string.Empty));
                PmdAppSetting.DeviceIdentifier = newDeviceId;
                return newDeviceId;
            }
            return registeredDeviceId;
        }

        PayMeDataStore dataStore = Data.PayMeDataStore.DefaultDataStore;
        MobileServiceAuthenticationProvider mobileServiceAuthenticationProvider;

    }
}
