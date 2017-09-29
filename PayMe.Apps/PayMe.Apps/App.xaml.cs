using PayMe.Apps.Helpers;
using PayMe.Apps.Models;
using PayMe.Apps.Services;
using PayMe.Apps.ViewModels;
using PayMe.Apps.Views;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PayMe.Apps
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();
            RegisterDataSubscriptions();
        }

        public static void SetMainPage()
        {
            var mainPage = new PrincipalMainPage
            {
                Title = Apps.Resources.Strings.AppName,
                Icon = null
            };

            Current.MainPage = mainPage;
        }

        public static void Init(IAuthentication authenticate)
        {
            Authenticator = authenticate;
        }

        private async void RegisterDataSubscriptions()
        {
            var dataStore = Data.PayMeDataStore.DefaultDataStore;

            await Task.Run(() =>
            {
                // Tags
                MessagingCenter.Subscribe<TagAddItemPage, Data.Entities.Tag>(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION, async (obj, item) =>
                {
                    var _item = item as Data.Entities.Tag;
                    await dataStore.SaveAsync(_item);
                });

                MessagingCenter.Subscribe<TagAddItemPage, Data.Entities.Tag>(this, ViewModelConstants.EDIT_ITEM_SUBSCRIPTION, async (obj, item) =>
                {
                    var _item = item as Data.Entities.Tag;
                    await dataStore.SaveAsync(_item);
                });

                //  Contacts
                MessagingCenter.Subscribe<ContactAddItemPage, Data.Entities.Contact>(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION, async (obj, item) =>
                {
                    var _item = item as Data.Entities.Contact;
                    await dataStore.SaveAsync(_item);
                });

                MessagingCenter.Subscribe<ContactAddItemPage, Data.Entities.Contact>(this, ViewModelConstants.EDIT_ITEM_SUBSCRIPTION, async (obj, item) =>
                {
                    var _item = item as Data.Entities.Contact;
                    await dataStore.SaveAsync(_item);
                });
            });
        }

        internal static IAuthentication Authenticator { get; private set; }

    }
}
