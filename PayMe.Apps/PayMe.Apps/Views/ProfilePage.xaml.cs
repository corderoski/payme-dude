using PayMe.Apps.Helpers;
using PayMe.Apps.Resources;
using PayMe.Apps.Services.Converters;
using PayMe.Apps.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        private StackLayout SecuredStackLayout;
        private readonly ProfileViewModel _viewModel;
        private readonly bool _isModalMode;

        public ProfilePage(bool isModalMode = false)
        {
            _isModalMode = isModalMode;
            _viewModel = new ProfileViewModel();

            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            Title = Strings.Label_Profile;
            SecuredStackLayout = new StackLayout { };

            RegisterUserSettingsPanel();
            RegisterUserInformationPanel();
            RegisterUserSigninPanel();

            _viewModel.AuthenticationRegistrationStarted += (sender, args) =>
            {
                SetRunningMode(true);
            };
            _viewModel.AuthenticationRegistrationCompleted += (sender, resultCode) =>
            {
                if (_isModalMode)
                {
                    this.SendBackButtonPressed();
                }
                SetRunningMode(false);

                if (resultCode != Models.DataStoreSyncCode.Success)
                    DisplayAlert(Strings.Label_Info, Strings.Message_Error_SyncError, Strings.Label_Okay);
            };

            ContentDescription.Children.Add(SecuredStackLayout);
            BindingContext = _viewModel;
        }

        private void RegisterUserSigninPanel()
        {
            var userSigninInformationLabel = new Label
            {
                FormattedText = new FormattedString(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            userSigninInformationLabel.FormattedText.Spans.Add(new Span { Text = Strings.Message_About_SinginInfoLabel_First });
            userSigninInformationLabel.FormattedText.Spans.Add(new Span
            {
                Text = string.Join(", ", AuthenticationHelper.Providers),
                FontAttributes = FontAttributes.Bold
            });
            userSigninInformationLabel.FormattedText.Spans.Add(new Span { Text = Strings.Message_About_SinginInfoLabel_Second });
            SecuredStackLayout.Children.Add(userSigninInformationLabel);

            var loginButton = ViewElementCustomSetting.GetDefaultPrimaryButton(text: Strings.Label_SignIn);
            loginButton.Command = _viewModel.DoLoginCommand;
            loginButton.SetBinding(IsVisibleProperty, "IsAuthenticationPending", mode: BindingMode.OneWay);
            SecuredStackLayout.Children.Add(loginButton);

            var signoutButton = ViewElementCustomSetting.GetDefaultPrimaryButton(
                text: Strings.Label_SignOut, color: ViewElementCustomSetting.GetColorFromGlobalResource("DangerRedColor"));
            signoutButton.SetBinding(IsVisibleProperty, new Binding("IsAuthenticationPending", BindingMode.Default,
                            new BooleanInverseValueConverter(), null));
            signoutButton.Clicked += async (sender, args) =>
            {
                var result = await DisplayAlert(Strings.Label_Warning_Title, Strings.Message_Profile_WarningEraseProfileData,
                                                Strings.Label_GotIt, Strings.Label_No);
                if (result)
                {
                    _viewModel.ExecuteUserLogout();
                }
            };
            SecuredStackLayout.Children.Add(signoutButton);
        }

        private void RegisterUserInformationPanel()
        {
            // info, stats
            var lastSyncDateLabel = new Label { FormattedText = new FormattedString() };
            lastSyncDateLabel.FormattedText.Spans.Add(new Span { Text = Strings.Label_Profile_LastSync });
            lastSyncDateLabel.FormattedText.Spans.Add(new Span { Text = "   " });
            lastSyncDateLabel.FormattedText.Spans.Add(new Span { Text = _viewModel.LastSyncDateString, FontAttributes = FontAttributes.Bold });
            lastSyncDateLabel.SetBinding(IsVisibleProperty, new Binding("IsAuthenticationPending", BindingMode.OneWay,
                            new BooleanInverseValueConverter(), null));
            SecuredStackLayout.Children.Add(lastSyncDateLabel);
        }

        private void RegisterUserSettingsPanel()
        {
            //  A grid for table-like formatting of settings controls
            var userCustomSettingsLayout = new Grid
            {
                Padding = 0,
                Margin = new Thickness(0, 10, 0, 5),
                RowSpacing = 1,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto }
                    },
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(8,  GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(2,  GridUnitType.Star) },
                    }

            };

            //  display empty transactions
            var displayEmptyTransactionsLabel = new Label { Text = Strings.Label_Settings_DisplayEmptyTransaction };
            userCustomSettingsLayout.Children.Add(displayEmptyTransactionsLabel, 0, 0);

            var displayEmptyTransactionsCheck = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            displayEmptyTransactionsCheck.SetBinding(Switch.IsToggledProperty, "DisplayEmptyTransactionGroups", mode: BindingMode.TwoWay);
            userCustomSettingsLayout.Children.Add(displayEmptyTransactionsCheck, 1, 0);

            // activate auto authentication
            var autoAuthenticationLabel = new Label { Text = Strings.Label_IsAutoAuthenticationEnabled };
            userCustomSettingsLayout.Children.Add(autoAuthenticationLabel, 0, 1);

            var autoAuthenticationLabelCheck = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            autoAuthenticationLabelCheck.SetBinding(Switch.IsToggledProperty, "EnableAutoAuthentication", mode: BindingMode.TwoWay);
            userCustomSettingsLayout.Children.Add(autoAuthenticationLabelCheck, 1, 1);

            // sharing message
            //var sharingMeessagesFormatLabel = new Label { Text = Strings.Label_Profile_CustomSharingPhrase };
            //userCustomSettingsLayout.Children.Add(sharingMeessagesFormatLabel, 0, 2);

            //var sharingMeessagesFormatEntry = new Entry
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.Start
            //};
            //sharingMeessagesFormatEntry.SetBinding(Entry.TextProperty, "CustomSharingPhrase", mode: BindingMode.TwoWay);
            //userCustomSettingsLayout.Children.Add(sharingMeessagesFormatEntry, 0, 3);

            //var sharingFormattedMessageResultEntry = new Label
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Editor)),
            //    HeightRequest = ViewElementCustomSetting.GetStandardEditorSize(),
            //};
            //sharingFormattedMessageResultEntry.SetBinding(Label.TextProperty, "CustomSharingFormattedMessage", mode: BindingMode.OneWay);
            //userCustomSettingsLayout.Children.Add(sharingFormattedMessageResultEntry, 0, 4);
            ////Grid.SetColumnSpan(sharingFormattedMessageResultEntry, 1);

            SecuredStackLayout.Children.Add(userCustomSettingsLayout);
        }

        private void SetRunningMode(bool isRunning)
        {
            BackgroundTaskIndicator.IsVisible = isRunning;
            BackgroundTaskIndicator.IsRunning = isRunning;
            ContentDescription.IsVisible = !isRunning;
        }

        protected override bool OnBackButtonPressed()
        {
            if (_isModalMode)
            {
                Navigation.PopModalAsync(true);
            }
            return base.OnBackButtonPressed();
        }

    }
}