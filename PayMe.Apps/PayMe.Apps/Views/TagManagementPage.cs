using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Resources;
using PayMe.Apps.ViewModels;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PayMe.Apps.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagManagementPage : ViewManagementPage<Tag>
    {

        public TagManagementPage()
        {
            this.Title = Strings.Label_Tags;

            InitializeSettings();
        }

        protected override async Task ToolbarItemNewEntity_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TagAddItemPage(ManagementPageModeType.Add), true);
        }

        protected override async Task ItemTappedForEdition_ClickedAsync(object sender, Tag item)
        {
            await Navigation.PushAsync(new TagAddItemPage(ManagementPageModeType.Edit, item), true);
        }

        private void InitializeSettings()
        {
            var listViewDataTemplate = new DataTemplate(() =>
            {
                var dataLabel = new Label
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextColor = ViewElementCustomSetting.GetColorFromGlobalResource("ListItemPrimaryStyle")
                };
                dataLabel.SetBinding(Label.TextProperty, new Binding("Name", BindingMode.OneWay));

                var stackLayout = new StackLayout() { Padding = new Thickness(8), Children = { dataLabel } };

                var commandForAnimation = new Command<View>(view =>
                {
                    view.DoScaleAnimation();
                });
                stackLayout.GestureRecognizers.Add(new TapGestureRecognizer { Command = commandForAnimation, CommandParameter = stackLayout });

                var commandForEditAction = new Command<Tag>(async tag =>
                {
                    await ItemTappedForEdition_ClickedAsync(this, tag);
                });
                var tapGestureForEditAction = new TapGestureRecognizer { Command = commandForEditAction };
                tapGestureForEditAction.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding(".", BindingMode.OneWay));
                stackLayout.GestureRecognizers.Add(tapGestureForEditAction);

                return new ViewCell { View = stackLayout };
            });

            ItemListView.ItemTemplate = listViewDataTemplate;
            ItemListView.RowHeight = Device.OnPlatform(50, 50, 60);

        }

    }
}