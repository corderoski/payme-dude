using PayMe.Apps.Data.Entities;
using PayMe.Apps.ViewModels;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using PayMe.Apps.Utils;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionItemView : ContentView
    {

        public bool IsEmpty { get => _itemViewModel.DataItems.Count <= 0; }

        public TransactionItemView(IGrouping<string, Transaction> item, TransactionType transactionType)
        {
            InitializeComponent();
            ClassId = item.Key;

            _itemViewModel = new TransactionItemViewModel(item, transactionType);
            _itemViewModel.OnItemViewTapped += ItemView_OnItemTapped;
            BindingContext = _itemViewModel;
        }

        public void AddSubItem(IEnumerable<Transaction> items)
        {
            _itemViewModel.AddRange(items);
        }

        public void RemoveSubItem(params Transaction[] items)
        {
            _itemViewModel.RemoveRange(items);
        }

        async void ItemView_OnItemTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransactionGroupingDetailPage(_itemViewModel.ContactName, _itemViewModel.DataItems), true);
        }

        private readonly TransactionItemViewModel _itemViewModel;

        public static TransactionItemView Transform(IGrouping<string, Transaction> item, TransactionType transactionType)
        {
            var view = new TransactionItemView(item, transactionType);
            var swipeGesture = new SwipeGestureRecognizer
            {
                CommandParameter = view,
                Direction = SwipeDirection.Right
            };
            swipeGesture.Swiped += (sender, args) =>
            {
                view.HeightRequest = view.Height + 20;
            };
            view.GestureRecognizers.Add(swipeGesture);
            return view;
        }

    }
}