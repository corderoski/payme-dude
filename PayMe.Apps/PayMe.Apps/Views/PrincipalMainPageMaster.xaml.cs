
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PayMe.Apps.ViewModels;
using PayMe.Apps.Resources;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalMainPageMaster : ContentPage
    {
        public ListView ListView;

        public PrincipalMainPageMaster()
        {
            InitializeComponent();

            BindingContext = new PrincipalMainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class PrincipalMainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PrincipalMainPageMenuItem> MenuItems { get; set; }

            public PrincipalMainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<PrincipalMainPageMenuItem>(new[]
                {
                    new PrincipalMainPageMenuItem(typeof(MainDashboardPage)) { Id = 0, Title = Strings.Label_Dashboard },
                    new PrincipalMainPageMenuItem(typeof(ContactManagementPage)) { Id = 1, Title = Strings.Label_Contacts },
                    new PrincipalMainPageMenuItem(typeof(TagManagementPage)) { Id = 2, Title = Strings.Label_Tags},
                    new PrincipalMainPageMenuItem(typeof(ProfilePage), false) { Id = 3, Title = Strings.Label_Profile },
                    new PrincipalMainPageMenuItem(typeof(AboutPage)) { Id = 5, Title = Strings.Label_Help },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}