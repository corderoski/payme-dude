using PayMe.Apps.Data.Entities;
using PayMe.Apps.Resources;
using PayMe.Apps.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainDashboardPage : TabbedPage
    {

        public MainDashboardPage()
        {
            Title = Strings.AppName;
            BarBackgroundColor = Helpers.ViewElementCustomSetting.GetColorFromGlobalResource("Primary");
            BarTextColor = Helpers.ViewElementCustomSetting.GetColorFromGlobalResource("LightBackgroundColor");

            _dashboardViewModel = new DashboardViewModel();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            myDebtsPage = new TransactionResumePage(_dashboardViewModel.MyDebtsNegativeTransactions,
                TransactionType.NegativeBalance, Strings.Label_Menu_PrimaryTab_MyDebts);
            theirDebtsPage = new TransactionResumePage(_dashboardViewModel.OtherDebtsPositiveTransactions, 
                TransactionType.PositiveBalance, Strings.Label_Menu_Secondary_OthersDebts);

            var newTransactionToolbarItem = new ToolbarItem
            {
                Icon = Device.OnPlatform(null, "add.png", null)
            };
            newTransactionToolbarItem.Clicked += async (sender, args) =>
            {
                var debtType = this.CurrentPage.Id == myDebtsPage.Id ? TransactionType.NegativeBalance : TransactionType.PositiveBalance;
                _dashboardViewModel.ActivateTransactionSubscription();
                await Navigation.PushAsync(new TransactionAddItemPage(debtType), true);
            };
            ToolbarItems.Add(newTransactionToolbarItem);

            var syncTransactionsToolbarItem = new ToolbarItem
            {
                Icon = Device.OnPlatform(null, "sync.png", null)
            };
            syncTransactionsToolbarItem.Clicked += _dashboardViewModel.SyncAction_ClickedAsync;
            ToolbarItems.Add(syncTransactionsToolbarItem);

            this.Padding = new Thickness(5, Device.OnPlatform(0, 0, 0), 5, 5);
            this.Children.Add(myDebtsPage);
            this.Children.Add(theirDebtsPage);
        }

        private readonly DashboardViewModel _dashboardViewModel;

        private TransactionResumePage myDebtsPage;
        private TransactionResumePage theirDebtsPage;

    }
}