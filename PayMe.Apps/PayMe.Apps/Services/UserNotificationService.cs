using PayMe.Apps.Resources;
using PayMe.Apps.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(UserNotificationService))]
namespace PayMe.Apps.Services
{

    public interface IUserNotificationService
    {
        Task DisplayMessage(string title, string message);
        Task<bool> DisplayQuestionMessage(string title, string message);
    }

    public class UserNotificationService : IUserNotificationService
    {

        public UserNotificationService() { }


        public async Task DisplayMessage(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, Strings.Label_Okay);
        }

        public async Task<bool> DisplayQuestionMessage(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, Strings.Label_Yes, Strings.Label_No);
        }

    }

}