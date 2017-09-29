using PayMe.Apps.Data;
using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Views;

using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PayMe.Apps.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {

        internal ObservableRangeCollection<Transaction> MyDebtsNegativeTransactions { get; }
        internal ObservableRangeCollection<Transaction> OtherDebtsPositiveTransactions { get; }

        internal event EventHandler OnCollectionRefreshed;

        /// <summary>
        /// Indicates if the Page pushed the New Item modal
        /// </summary>
        public bool IsShowingAddItemView { get; set; }

        public DashboardViewModel()
        {
            dataStore = PayMeDataStore.DefaultDataStore;
            InitializeAsync().ConfigureAwait(false);

            MyDebtsNegativeTransactions = new ObservableRangeCollection<Transaction>();
            OtherDebtsPositiveTransactions = new ObservableRangeCollection<Transaction>();
        }

        internal void ActivateTransactionSubscription()
        {
            IsShowingAddItemView = true;
            MessagingCenter.Subscribe<TransactionAddItemPage, Transaction>(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION, async (obj, item) =>
            {
                var _item = item as Transaction;
                await dataStore.SaveAsync(_item);

                if (_item.Type == TransactionType.NegativeBalance)
                    MyDebtsNegativeTransactions.Add(_item);
                else
                    OtherDebtsPositiveTransactions.Add(_item);

                IsShowingAddItemView = false;
                DeactivateTransactionSubscription();
            });
        }

        internal void DeactivateTransactionSubscription()
        {
            if (!IsShowingAddItemView)
            {
                MessagingCenter.Unsubscribe<TransactionAddItemPage, Transaction>(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAdding">true if adding, otherwise, replace all.</param>
        /// <param name="reloadFromServer">whether must sync and reload from server or not</param>
        /// <returns></returns>
        internal async Task ResfreshDataStorageAsync(bool isAdding, bool reloadFromServer = false)
        {
            var dataStoreSyncResult = await dataStore.GetItemsAsync<Transaction>(reloadFromServer, includeNavigationProperties: true);
            if (dataStoreSyncResult.Code == Models.DataStoreSyncCode.Success)
            {
                if (!isAdding)
                {
                    MyDebtsNegativeTransactions.ReplaceRange(dataStoreSyncResult.Items.Where(p => p.Type == TransactionType.NegativeBalance));
                    OtherDebtsPositiveTransactions.ReplaceRange(dataStoreSyncResult.Items.Where(p => p.Type == TransactionType.PositiveBalance));
                }
                else
                {
                    MyDebtsNegativeTransactions.AddRange(dataStoreSyncResult.Items.Where(p => p.Type == TransactionType.NegativeBalance));
                    OtherDebtsPositiveTransactions.AddRange(dataStoreSyncResult.Items.Where(p => p.Type == TransactionType.PositiveBalance));
                }
            }
        }

        internal async void SyncAction_ClickedAsync(object sender, EventArgs e)
        {
            await ResfreshDataStorageAsync(false, true);
        }

        async Task InitializeAsync()
        {
            await ResfreshDataStorageAsync(true, false);

            OnCollectionRefreshed?.Invoke(this, new EventArgs());
            MessagingCenter.Subscribe<TransactionGroupingDetailPage, Transaction>(this,
                ViewModelConstants.REMOVE_ITEM_SUBSCRIPTION,
                async (obj, item) =>
                {

                    var userNotificationService = DependencyService.Get<Services.IUserNotificationService>(DependencyFetchTarget.GlobalInstance);
                    var dialogResult = await userNotificationService.DisplayQuestionMessage(
                                                Resources.Strings.Message_Warning_WaitTitle,
                                                Resources.Strings.Message_Transactions_ActionWillDeleteTransaction);

                    if (dialogResult)
                    {
                        var _item = item as Transaction;
                        if (_item.Type == TransactionType.NegativeBalance)
                            MyDebtsNegativeTransactions.Remove(_item);
                        else
                            OtherDebtsPositiveTransactions.Remove(_item);

                        await dataStore.RemoveAsync(_item);
                    }
                });
        }

        readonly PayMeDataStore dataStore;
    }
}
