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
    public partial class AboutPage : ContentPage
    {

        public AboutPage()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            Title = Strings.Label_Help;

            RegisterAppInformationPanel();
            RegisterHelpInformationPanel();
        }

        private void RegisterAppInformationPanel()
        {
            var appAssemblyInformationLabel = new Label { FormattedText = new FormattedString(), FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };
            appAssemblyInformationLabel.FormattedText.Spans.Add(new Span { Text = Strings.AppName, FontAttributes = FontAttributes.Bold });
            appAssemblyInformationLabel.FormattedText.Spans.Add(new Span { Text = "   " });
#if DEBUG
            appAssemblyInformationLabel.FormattedText.Spans.Add(new Span { Text = $"{PmdAppSetting.Version} [DEBUG]", ForegroundColor = ViewElementCustomSetting.GetColorFromGlobalResource("LightTextColor") });
#else
            appAssemblyInformationLabel.FormattedText.Spans.Add(new Span { Text = PmdAppSetting.Version, ForegroundColor = ViewElementCustomSetting.GetColorFromGlobalResource("LightTextColor") });
#endif
            ContentDescription.Children.Add(appAssemblyInformationLabel);

            var developerInformationLabel = new Label { FormattedText = new FormattedString() };
            developerInformationLabel.FormattedText.Spans.Add(new Span { Text = Strings.Label_DevelopedBy });
            developerInformationLabel.FormattedText.Spans.Add(new Span { Text = " " });
            developerInformationLabel.FormattedText.Spans.Add(new Span { Text = Strings.AppAuthor, FontAttributes = FontAttributes.Bold });
            developerInformationLabel.FormattedText.Spans.Add(new Span { Text = " | " });
            developerInformationLabel.FormattedText.Spans.Add(new Span { Text = "Icons by Icons8" });
            developerInformationLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => Device.OpenUri(new Uri("http://corderoski.com"))) });
            ContentDescription.Children.Add(developerInformationLabel);
        }

        private void RegisterHelpInformationPanel()
        {
            ContentDescription.Children.Add(new Label
            {
                Text = Strings.Label_HowTo,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            var tagsHelpLabel = new Label { FormattedText = new FormattedString() };
            tagsHelpLabel.FormattedText.Spans.Add(new Span
            {
                Text = Strings.Label_Tags,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            tagsHelpLabel.FormattedText.Spans.Add(new Span { Text = Environment.NewLine });
            tagsHelpLabel.FormattedText.Spans.Add(new Span { Text = Strings.Message_Help_TagsDescription });
            ContentDescription.Children.Add(tagsHelpLabel);

            var contactsHelpLabel = new Label { FormattedText = new FormattedString() };
            contactsHelpLabel.FormattedText.Spans.Add(new Span
            {
                Text = Strings.Label_Contacts,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            contactsHelpLabel.FormattedText.Spans.Add(new Span { Text = Environment.NewLine });
            contactsHelpLabel.FormattedText.Spans.Add(new Span { Text = Strings.Message_Help_ContactsDescription });
            ContentDescription.Children.Add(contactsHelpLabel);

            var dashboardHelpLabel = new Label { FormattedText = new FormattedString() };
            dashboardHelpLabel.FormattedText.Spans.Add(new Span
            {
                Text = Strings.Label_Dashboard,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });
            dashboardHelpLabel.FormattedText.Spans.Add(new Span { Text = Environment.NewLine });
            dashboardHelpLabel.FormattedText.Spans.Add(new Span { Text = Strings.Message_Help_DashboardDescription });
            ContentDescription.Children.Add(dashboardHelpLabel);
        }

    }
}