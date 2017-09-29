using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Resources;
using PayMe.Apps.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactAddItemPage : ContentPage
    {

        public ContactAddItemPage(ManagementPageModeType pageModeType) : this(pageModeType, null) { }

        public ContactAddItemPage(ManagementPageModeType pageModeType, Contact contact = null)
        {
            _editingEntity = contact;
            _pageModeType = pageModeType;
            InitializeSettings();
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
            ToolbarItems.Add(addToolBarItem);

            nameEntryControl = new Entry { Keyboard = Keyboard.Text };
            notesEntryControl = new Editor { Keyboard = Keyboard.Default, HeightRequest = ViewElementCustomSetting.GetStandardEditorSize() };
            var sLayout = new StackLayout
            {
                Padding = ViewElementCustomSetting.GetStandardThickness(),
                Spacing = 10,
                Children =
                {
                    new Label { Text = Strings.Label_Name },
                    nameEntryControl,
                    new Label { Text = Strings.Label_Notes },
                    notesEntryControl,

                    addButton
                }
            };
            Content = sLayout;

            if (_pageModeType == ManagementPageModeType.Edit)
            {
                nameEntryControl.Text = _editingEntity.Name;
                notesEntryControl.Text = _editingEntity.Notes;

                var removeToolBarItem = new ToolbarItem { Icon = Device.OnPlatform(null, "clear.png", null) };
                removeToolBarItem.Clicked += (sender, args) =>
                {
                    MessagingCenter.Send(this, ViewModelConstants.REMOVE_ITEM_SUBSCRIPTION, _editingEntity);
                };
                ToolbarItems.Insert(0, removeToolBarItem);
            }
        }

        async void AddToolBarItemSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntryControl.Text) || string.IsNullOrWhiteSpace(nameEntryControl.Text))
            {
                await DisplayAlert(Strings.Message_Warning_WaitTitle, Strings.Message_CannotSaveEmptyItems, Strings.Label_GotIt);
                return;
            }

            if (_pageModeType == ManagementPageModeType.Edit)
            {
                _editingEntity.Name = nameEntryControl.Text;
                _editingEntity.Notes = notesEntryControl.Text;
                MessagingCenter.Send(this, ViewModelConstants.EDIT_ITEM_SUBSCRIPTION, _editingEntity);
            }
            else
            {
                string contactIdOnDevice = Guid.NewGuid().ToString();
                MessagingCenter.Send(
                    this,
                    ViewModelConstants.ADD_ITEM_SUBSCRIPTION,
                    new Contact
                    {
                        Name = nameEntryControl.Text,
                        Notes = notesEntryControl.Text,
                        DeviceUniqueContactId = contactIdOnDevice
                    });
            }
            await Navigation.PopToRootAsync(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<ContactAddItemPage, Contact>(this, ViewModelConstants.REMOVE_ITEM_SUBSCRIPTION, async (obj, item) =>
            {
                var _item = item as Contact;
                var result = await Data.PayMeDataStore.DefaultDataStore.RemoveAsync(_item);
                if (result == Models.DataStoreOperationResult.CannotDeleteDueToRelatedEntities)
                {
                    await DisplayAlert(
                        Strings.Message_Warning_WaitTitle,
                        Strings.Message_Error_CannotDeleteDueToRelatedEntities,
                        Strings.Label_GotIt);
                }
                else
                {
                    await Navigation.PopToRootAsync(true);
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ContactAddItemPage, Contact>(this, ViewModelConstants.REMOVE_ITEM_SUBSCRIPTION);
        }

        private Entry nameEntryControl;
        private Editor notesEntryControl;

        private readonly Contact _editingEntity;
        private readonly ManagementPageModeType _pageModeType;

    }
}