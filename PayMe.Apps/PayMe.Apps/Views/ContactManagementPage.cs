using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Resources;
using PayMe.Apps.Utils;
using PayMe.Apps.ViewModels;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactManagementPage : ViewManagementPage<Contact>
    {

        public ContactManagementPage()
        {
            this.Title = Strings.Label_Contacts;

            InitializeSettings();
        }

        protected override async Task ToolbarItemNewEntity_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactAddItemPage(ManagementPageModeType.Add), true);
        }

        protected override async Task ItemTappedForEdition_ClickedAsync(object sender, Contact item)
        {
            await Navigation.PushAsync(new ContactAddItemPage(ManagementPageModeType.Edit, item), true);
        }

        private void InitializeSettings()
        {
            var listViewDataTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label
                {
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = ViewElementCustomSetting.GetColorFromGlobalResource("ListItemPrimaryStyle")
                };
                nameLabel.SetBinding(Label.TextProperty, new Binding("Name", BindingMode.OneWay));

                var notesLabel = new Label
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextColor = ViewElementCustomSetting.GetColorFromGlobalResource("ListItemSecondaryStyle")
                };
                notesLabel.SetBinding(Label.TextProperty, new Binding("Notes", BindingMode.OneWay));

                var minimumFixedHeightSize = nameLabel.FontSize + notesLabel.FontSize + 2;
                var stackLayout = new StackLayout
                {
                    Padding = 0,
                    Margin = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    MinimumHeightRequest = minimumFixedHeightSize,
                    Children = {
                        nameLabel,
                        notesLabel
                    }
                };

                var commandForAnimation = new Command<View>(view =>
                {
                    view.DoScaleAnimation();
                });
                stackLayout.GestureRecognizers.Add(new TapGestureRecognizer { Command = commandForAnimation, CommandParameter = stackLayout });

                var commandForEditAction = new Command<Contact>(async e =>
                {
                    await ItemTappedForEdition_ClickedAsync(this, e);
                });
                var tapGestureForEditAction = new TapGestureRecognizer { Command = commandForEditAction };
                tapGestureForEditAction.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding(".", BindingMode.OneWay));
                stackLayout.GestureRecognizers.Add(tapGestureForEditAction);

                return new ViewCell { View = stackLayout };
            });
            ItemListView.ItemTemplate = listViewDataTemplate;
            ItemListView.HasUnevenRows = true;
        }

    }
}