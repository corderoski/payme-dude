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
    public partial class TagAddItemPage : ContentPage
    {

        public TagAddItemPage(ManagementPageModeType pageModeType) : this(pageModeType, null) { }

        public TagAddItemPage(ManagementPageModeType pageModeType, Tag tag = null)
        {
            _editingEntity = tag;
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

            addButton.Clicked += AddItemSaveAction_Clicked;
            addToolBarItem.Clicked += AddItemSaveAction_Clicked;
            this.ToolbarItems.Add(addToolBarItem);

            nameEntryControl = new Entry { Keyboard = Keyboard.Text };
            var sLayout = new StackLayout
            {
                Padding = ViewElementCustomSetting.GetStandardThickness(),
                Spacing = 10,
                Children =
                {
                    new Label { Text = Strings.Label_Name },
                    nameEntryControl,

                    addButton
                }
            };
            Content = sLayout;

            if (_pageModeType == ManagementPageModeType.Edit)
            {
                nameEntryControl.Text = _editingEntity.Name;
            }
        }

        async void AddItemSaveAction_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntryControl.Text) || string.IsNullOrWhiteSpace(nameEntryControl.Text))
            {
                await DisplayAlert(Strings.Message_Warning_WaitTitle, Strings.Message_CannotSaveEmptyItems, Strings.Label_GotIt);
                return;
            }

            if (_pageModeType == ManagementPageModeType.Edit)
            {
                _editingEntity.Name = nameEntryControl.Text;
                MessagingCenter.Send(this, ViewModelConstants.EDIT_ITEM_SUBSCRIPTION, _editingEntity);
            }
            else
            {
                MessagingCenter.Send(this, ViewModelConstants.ADD_ITEM_SUBSCRIPTION, new Tag { Name = nameEntryControl.Text });
            }
            await Navigation.PopToRootAsync(true);
        }

        private Entry nameEntryControl;

        private Tag _editingEntity { get; }
        private readonly ManagementPageModeType _pageModeType;

    }
}