using Xamarin.Forms;

namespace PayMe.Apps.Helpers
{
    public static class ViewElementCustomAnimation
    {

        public static async void DoScaleAnimation(this View viewElement)
        {
            await viewElement.ScaleTo(0.95, 50, Easing.CubicOut);
            await viewElement.ScaleTo(1, 50, Easing.CubicIn);
        }

    }
}
