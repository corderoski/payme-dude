using Microsoft.WindowsAzure.MobileServices;
using PayMe.Apps.Data;
using PayMe.Apps.Helpers;
using PayMe.Apps.Models;
using PayMe.Apps.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PayMe.Apps.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        /// <summary>
        /// Indicates the initialization of the Authentication process.
        /// </summary>
        public event EventHandler AuthenticationRegistrationStarted;
        /// <summary>
        /// Indicates the completition of the Authentication and his success or failure.
        /// </summary>
        public event EventHandler<DataStoreSyncCode> AuthenticationRegistrationCompleted;

        public ICommand DoLoginCommand { get; }

        private bool _isAuthenticationPending;
        public bool IsAuthenticationPending
        {
            get => _isAuthenticationPending;
            private set => SetProperty(ref _isAuthenticationPending, value);
        }

        public string LastSyncDateString
        {
            get
            {
                var lastRegisteredSync = PmdAppSetting.LastSuccessfulSync;
                return lastRegisteredSync > DateTime.MinValue ? lastRegisteredSync.ToString() : Resources.Strings.Label_SyncExecution_None;
            }
        }

        public bool DisplayEmptyTransactionGroups
        {
            get { return PmdAppSetting.ShowZeroBasedTransactions; }
            set { PmdAppSetting.ShowZeroBasedTransactions = value; }
        }

        public bool EnableAutoAuthentication
        {
            get { return PmdAppSetting.IsAutoAuthenticationEnabled; }
            set { PmdAppSetting.IsAutoAuthenticationEnabled = value; }
        }

        public string CustomSharingPhrase
        {
            get { return PmdAppSetting.SharingMessageCustomPhrase; }
            set { PmdAppSetting.SharingMessageCustomPhrase = value; }
        }

        public string CustomSharingFormattedMessage
        {
            get => string.Format(Resources.Strings.User_Setting_SharingMessageFormat,
                          PmdAppSetting.SharingMessageCustomPhrase,
                          99.99.ToString(Services.Converters.CurrencyStringValueConverter.CURRENCY_STRING_FORMAT));
        }

        public ProfileViewModel()
        {
            IsAuthenticationPending = !PmdAppSetting.IsAccountRegistrationCompleted;

            _deviceConnectionService = DependencyService.Get<IDeviceConnectionHelper>(DependencyFetchTarget.GlobalInstance);
            _authenticationService = DependencyService.Get<IAuthenticationService>(DependencyFetchTarget.GlobalInstance);
            _authenticationService.RegistrationStarted += (sender, args) =>
            {
                AuthenticationRegistrationStarted?.Invoke(sender, args);
            };
            _authenticationService.RegistrationCompleted += (sender, code) =>
            {
                AuthenticationRegistrationCompleted?.Invoke(sender, code);
                IsAuthenticationPending = !PmdAppSetting.IsAccountRegistrationCompleted;
            };

            DoLoginCommand = new Command(() => DispatchLogin_CommandClick(this, new EventArgs()));
        }

        public async void ExecuteUserLogout()
        {
            await dataStore.CurrentClient.LogoutAsync();
            PmdAppSetting.DeleteUserData();
            IsAuthenticationPending = true;
        }

        async void DispatchLogin_CommandClick(object sender, EventArgs e)
        {
            //  1 - connected, 2 - not connected, 3 - error due to permission
            var isDeviceConnected = 0;
            try
            {
                isDeviceConnected = await _deviceConnectionService.IsNetworkConnectedAsync() ? 1 : 2;
            }
            catch (DevicePermissionNotLocatedException)
            {
                var userNotificationService = DependencyService.Get<IUserNotificationService>(DependencyFetchTarget.GlobalInstance);
                await userNotificationService.DisplayMessage(Resources.Strings.Label_Error_SyncError_Title,
                                                            Resources.Strings.Message_Error_DeviceWithoutNetworkPermission);
                return;
            }

            await _authenticationService.DoAuthenticationAsync();
        }

        PayMeDataStore dataStore = Data.PayMeDataStore.DefaultDataStore;
        readonly IDeviceConnectionHelper _deviceConnectionService;
        readonly IAuthenticationService _authenticationService;

    }
}
