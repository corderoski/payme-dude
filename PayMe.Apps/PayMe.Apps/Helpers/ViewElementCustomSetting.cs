
using System;
using System.Linq;
using Xamarin.Forms;

namespace PayMe.Apps.Helpers
{
    internal static class ViewElementCustomSetting
    {

        public static Color GetColorFromGlobalResource(string name)
        {
            return (Color)Application.Current.Resources[name];
        }

        public static ActivityIndicator GetDefaultActivityIndicator()
        {
            return new ActivityIndicator
            {
                IsVisible = false,
                IsRunning = false,
                IsEnabled = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Color = GetColorFromGlobalResource("BackGroundIndicatorStyle")
            };
        }

        public static Button GetDefaultPrimaryButton(string text = "", Color? color = null)
        {
            return new Button
            {
                Text = text,
                Margin = new Thickness(0, 10, 0, 0),
                BackgroundColor = color.HasValue ? color.Value : (Color)Application.Current.Resources["Primary"],
                TextColor = (Color)Application.Current.Resources["PrimaryTextColor"]
            };
        }

        public static ScrollView GetDefaultScrollView()
        {
            return new ScrollView
            {
                Padding = 0,
                Margin = 0,
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
        }

        public static Thickness GetStandardThickness()
        {
            return new Thickness(5);
        }

        public static double GetStandardEditorSize()
        {
            //Device.GetNamedSize(NamedSize.Default, typeof(Editor)) * 2,
            return 96;
        }

        public static View GetEmptyLabelSpace(int spaceEquivalency = 1)
        {
            var emptyLabel = new Label { FormattedText = new FormattedString() };
            for (int i = 0; i < spaceEquivalency; i++)
                emptyLabel.FormattedText.Spans.Add(new Span
                {
                    Text = Environment.NewLine,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                });
            return emptyLabel;
        }

        public static void SetPrimaryButtonStyle(this Button button)
        {
            button.BackgroundColor = GetColorFromGlobalResource("Primary");
            button.TextColor = GetColorFromGlobalResource("LightBackgroundColor");
        }
    }
}