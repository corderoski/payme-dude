using PayMe.Apps.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalMainPage : MasterDetailPage
    {

        public PrincipalMainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            _mainNavigationPage = new NavigationPage(new MainDashboardPage());
            this.Detail = _mainNavigationPage;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PrincipalMainPageMenuItem;
            if (item == null)
                return;

            if (item.TargetType == typeof(MainDashboardPage))
            {
                this.Detail = _mainNavigationPage;
            }
            else
            {
                var page = item.Parameters == null ? (Page)Activator.CreateInstance(item.TargetType) : 
                                                        (Page)Activator.CreateInstance(item.TargetType, item.Parameters);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
            }

            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;
        }

        private readonly NavigationPage _mainNavigationPage;

    }
}