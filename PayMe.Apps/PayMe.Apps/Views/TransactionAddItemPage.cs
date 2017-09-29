using PayMe.Apps.Data;
using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Resources;
using PayMe.Apps.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionAddItemPage : ContentPage
    {

        public TransactionAddItemPage(TransactionType transactionType)
        {
            _transactionType = transactionType;
            InitializeSettings();

            InitializeDataAsync();
        }

        private void InitializeSettings()
        {
            var addButton = new Button
            {
                Text = ViewModelConstants.CHECKPOINT_SYMBOL,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = Width
            };
            addButton.SetPrimaryButtonStyle();

            var addToolBarItem = new ToolbarItem { Icon = Device.OnPlatform(null, "check_bar.png", null) };

            addButton.Clicked += AddToolBarItemSave_Clicked;
            addToolBarItem.Clicked += AddToolBarItemSave_Clicked;
            this.ToolbarItems.Add(addToolBarItem);

            // amount input
            amountEntryControl = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = ViewModelConstants.INPUT_CURRENCY_PLACEHOLDER
            };
            // description input
            descriptionEntryControl = new Editor
            {
                Keyboard = Keyboard.Text,
                HeightRequest = ViewElementCustomSetting.GetStandardEditorSize()
            };
            // contacts picker
            contactPicker = new Picker
            {
                Title = Strings.Label_Contacts,
                IsVisible = true,
                TextColor = ViewElementCustomSetting.GetColorFromGlobalResource("ListItemSecondaryStyle")
            };
            contactPicker.ItemDisplayBinding = new Binding("Name");
            contactPicker.SelectedIndexChanged += Picker_SelectedIndexChanged;

            var sLayout = new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
                Spacing = 10,
                Children = {
                        new Label { Text = ViewModelConstants.INPUT_CURRENCY_SYMBOL },
                        amountEntryControl,

                        new Label { Text = Strings.Label_Description},
                        descriptionEntryControl,

                        new Label {  Text = Strings.Label_Contact },
                        contactPicker,

                        addButton
                    }
            };
            Content = sLayout;
        }

        async void InitializeDataAsync()
        {
            var dataStore = PayMeDataStore.DefaultDataStore;
            var contactResult = await dataStore.GetItemsAsync<Contact>(false);
            if (!contactResult.Items.Any())
            {
                await DisplayAlert(Strings.Message_Warning_StopTitle,
                    Strings.Message_Transactions_ContactsNotFoundAndCreate, Strings.Label_GotIt);
                Navigation.RemovePage(this);
                return;
            }
            contactPicker.ItemsSource = contactResult.Items;
        }

        void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{contactPicker.SelectedItem}");
        }

        async void AddToolBarItemSave_Clicked(object sender, EventArgs e)
        {
            var selection = contactPicker.SelectedItem;
            if (selection == null)
            {
                if (contactPicker.ItemsSource.Count <= 0)
                    await DisplayAlert(Strings.Message_Warning_WaitTitle, Strings.Message_Transactions_ContactsNotFoundAndCreate, Strings.Label_GotIt);
                else
                    await DisplayAlert(Strings.Message_Warning_WaitTitle, Strings.Message_Transactions_MustSelectAContact, Strings.Label_GotIt);
                return;
            }

            var item = (Contact)selection;

            decimal.TryParse(amountEntryControl.Text, out decimal amount);
            var debtType = _transactionType;

            var newTransaction = new Transaction
            {
                Amount = amount,
                Description = descriptionEntryControl.Text,
                ContactId = item.Id,
                Type = debtType,
            };

            MessagingCenter.Send(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION, newTransaction);
            await Navigation.PopToRootAsync(true);
        }

        private Entry amountEntryControl;
        private Editor descriptionEntryControl;
        private Picker contactPicker;

        private readonly TransactionType _transactionType;

    }
}