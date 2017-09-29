using Xamarin.Forms;

namespace PayMe.Apps.Utils
{
    public interface ISwipeGestureController
    {
        void SendSwipe(Element sender, double totalX, double totalY);
        bool DetectSwipe(View sender, SwipeDirection direction);
    }
}
