using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace PayMe.Apps.Helpers
{
    public static class PmdAppSetting
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// Checks requirements for a complete registration.
        /// </summary>
        public static bool IsAccountRegistrationCompleted
        {
            get { return IsProviderAuthenticated & !string.IsNullOrEmpty(RegistrationId); }
        }

        /// <summary>
        /// Whether the user is authenticated (with an external sign-on provider).
        /// </summary>
        public static bool IsProviderAuthenticated {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsProviderAuthenticated), false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsProviderAuthenticated), value);
            }
        }

        /// <summary>
        /// Resolved token from the server after authentication
        /// </summary>
        public static string RegistrationId
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(RegistrationId), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(RegistrationId), value);
            }
        }

        /// <summary>
        /// Resolved temp token from Auth Provider
        /// </summary>
        public static string UserProviderAuthentication
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(UserProviderAuthentication), null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(UserProviderAuthentication), value);
            }
        }

        /// <summary>
        /// Unique Device identifier for an user.
        /// </summary>
        public static string DeviceIdentifier
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(DeviceIdentifier), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(DeviceIdentifier), value);
            }
        }

        /// <summary>
        /// Last successful connection to the server
        /// </summary>
        public static DateTime LastSuccessfulSync
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(LastSuccessfulSync), DateTime.MinValue);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(LastSuccessfulSync), value);
            }
        }

        /// <summary>
        /// Indicates whether the user wants to show empty or zero-based transactions
        /// </summary>
        public static bool ShowZeroBasedTransactions
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(ShowZeroBasedTransactions), true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(ShowZeroBasedTransactions), value);
            }
        }

        /// <summary>
        /// Indicates whether the user wants the app to authenticate by itself when required
        /// </summary>
        public static bool IsAutoAuthenticationEnabled
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(IsAutoAuthenticationEnabled), true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(IsAutoAuthenticationEnabled), value);
            }
        }

        public static string SharingMessageCustomPhrase
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(SharingMessageCustomPhrase), string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(SharingMessageCustomPhrase), value);
            }
        }

        public const string Version = "0.0.1";

        public static void DeleteUserData()
        {
            AppSettings.Remove(nameof(IsProviderAuthenticated));
            AppSettings.Remove(nameof(RegistrationId));
            AppSettings.Remove(nameof(UserProviderAuthentication));
            AppSettings.Remove(nameof(LastSuccessfulSync));
            AppSettings.Remove(nameof(ShowZeroBasedTransactions));
            AppSettings.Remove(nameof(IsAutoAuthenticationEnabled));
            AppSettings.Remove(nameof(SharingMessageCustomPhrase));
        }

    }
}