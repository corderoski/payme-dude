using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using System;

namespace PayMe.Apps.Views
{
    public class TransactionResumePage : ContentPage
    {

        public TransactionResumePage(ObservableRangeCollection<Transaction> collection, TransactionType transactionType, string title)
        {
            Title = title;
            _transactionType = transactionType;

            InitializeSettings();
            InitializeData(collection);
        }

        private void InitializeSettings()
        {
            resumeDashboardGrid = new Grid
            {
                Padding = 0,
                Margin = 0,
                RowSpacing = 0,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            stackLayout.Children.Add(resumeDashboardGrid);

            var scrollView = ViewElementCustomSetting.GetDefaultScrollView();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private void InitializeData(ObservableRangeCollection<Transaction> initialObservableCollection)
        {
            _groupingItemViews = new ObservableCollection<TransactionItemView>();

            initialObservableCollection.CollectionChanged += Items_CollectionChanged;
            if (initialObservableCollection.Any())
            {
                resumeDashboardGrid.Children.AddVertical(Transform(initialObservableCollection));
            }
        }

        private TransactionItemView RegisterTransformation(IGrouping<string, Transaction> p)
        {
            var view = TransactionItemView.Transform(p, _transactionType);
            _groupingItemViews.Add(view);
            return view;
        }

        private IEnumerable<TransactionItemView> Transform(IEnumerable<Transaction> transactions)
        {
            var groupsByContact = TransactionItemViewModel.CreateGrouping(transactions);
            var views = new List<TransactionItemView>();
            foreach (var item in groupsByContact)
            {
                views.Add(RegisterTransformation(item));
            }
            return views;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var transactions = e.NewItems.OfType<Transaction>();

                //  looks for ViewItems with an existing Contact
                {
                    var addingItems_iterator = _groupingItemViews.Where(p => transactions.Any(t => t.Contact.Name == p.ClassId));
                    foreach (var item in addingItems_iterator)
                    {
                        //  sends the Transactions for a specific contact to his view
                        item.AddSubItem(transactions.Where(p => p.Contact.Name == item.ClassId));
                    }
                }

                //  looks for transactions with a new registered Contact
                {
                    var newItems_iterator = transactions.Where(p => !_groupingItemViews.Any(t => t.ClassId == p.Contact.Name));
                    var views = TransactionItemViewModel.CreateGrouping(newItems_iterator).Select(p =>
                    {
                        return RegisterTransformation(p);
                    }).ToList();
                    if (views.Any())
                        resumeDashboardGrid.Children.AddVertical(views);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var transaction = item as Transaction;

                    var items_iterator = _groupingItemViews.FirstOrDefault(p => p.ClassId == transaction.Contact.Name);
                    items_iterator.RemoveSubItem(transaction);

                    if(items_iterator.IsEmpty)
                    {
                        resumeDashboardGrid.Children.Remove(items_iterator);
                    }
                }
            }
        }

        private readonly TransactionType _transactionType;

        private ObservableCollection<TransactionItemView> _groupingItemViews;

        private Grid resumeDashboardGrid;

    }
}
