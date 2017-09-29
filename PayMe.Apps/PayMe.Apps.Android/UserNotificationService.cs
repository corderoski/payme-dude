

using Android.App;
using PayMe.Apps.Droid;
using PayMe.Apps.Services;

[assembly: Xamarin.Forms.Dependency(typeof(UserNotificationService))]
namespace PayMe.Apps.Droid
{
    public class UserNotificationService : Activity, IUserNotificationService
    {

        public UserNotificationService() { }

        public void DisplayMessage(string title, string message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this.ApplicationContext);
            builder.SetMessage(message);
            builder.SetTitle(title);
            var alert = builder.Create();
            alert.Show();
        }

    }

}