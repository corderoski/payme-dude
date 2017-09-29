using PayMe.Apps.Data.Entities;
using PayMe.Apps.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionGroupingDetailPage : ContentPage
    {

        public TransactionGroupingDetailPage(string key, IEnumerable<Transaction> dataItems)
        {
            Title = key;
            InitializeComponent();

            DetailListView.ItemsSource = dataItems;
        }

        void DetailListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            ((ListView)sender).SelectedItem = null;
        }

        void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender == null || !(sender is Image)) return;

            var data = (sender as Image).BindingContext;

            MessagingCenter.Send(this, ViewModelConstants.REMOVE_ITEM_SUBSCRIPTION, data as Transaction);
        }
    }
}