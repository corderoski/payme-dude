using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace PayMe.Apps.ViewModels
{
    public class TransactionItemViewModel : BaseViewModel
    {

        public string ContactName { get => _key; }

        private string _amount;
        public string Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value, nameof(Amount));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value, nameof(Description));
            }
        }

        public Color NumericColorString { get; }
        public ICommand RowTappedCommand { get; }

        private ObservableRangeCollection<Transaction> _items;
        public ObservableRangeCollection<Transaction> DataItems
        {
            get => _items;
            set
            {
                SetProperty(ref _items, value, nameof(DataItems));
            }
        }

        public event EventHandler OnItemViewTapped;

        public TransactionItemViewModel(IGrouping<string, Transaction> item, TransactionType transactionType)
        {
            _key = item.Key;
            _items = new ObservableRangeCollection<Transaction>(item.ToList());

            NumericColorString = transactionType == TransactionType.NegativeBalance ?
                                                                    ViewElementCustomSetting.GetColorFromGlobalResource("NegativeBalanceStyle") :
                                                                    ViewElementCustomSetting.GetColorFromGlobalResource("PositiveBalanceStyle");
            SetGetterStrings();
            //
            RowTappedCommand = new Command<View>(RowTapped);
        }

        public void AddRange(IEnumerable<Transaction> items)
        {
            DataItems.AddRange(items);
            SetGetterStrings();
        }

        public void RemoveRange(IEnumerable<Transaction> items)
        {
            foreach (var item in items)
            {
                DataItems.Remove(item);
            }
            SetGetterStrings();
        }

        public double GetStandardSize()
        {
            return MinimumHeightByItem * DataItems.Count;
        }

        void SetGetterStrings()
        {
            Amount = _items.Sum(p => p.Amount).ToString(Services.Converters.CurrencyStringValueConverter.CURRENCY_STRING_FORMAT);

            Description = string.Format(_items.Count != 1 ?
                            Resources.Strings.Label_Transaction_TransactionsCountFormatted :
                                Resources.Strings.Label_Transaction_OneTransactionCountFormatted,
                            _items.Count);
        }

        void RowTapped(View item)
        {
            item.DoScaleAnimation();
            OnItemViewTapped?.Invoke(item, new EventArgs());
        }

        private string _key;

        public static IEnumerable<IGrouping<string, Transaction>> CreateGrouping(IEnumerable<Transaction> transactions)
        {
            return transactions.GroupBy(p => p.Contact.Name);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        private double MinimumHeightByItem { get => Device.OnPlatform(68, 68, 80); }
#pragma warning restore CS0618 // Type or member is obsolete

    }
}
