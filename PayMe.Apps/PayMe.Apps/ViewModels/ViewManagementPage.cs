using System.Collections.ObjectModel;
using PayMe.Apps.Data.Entities;
using System.Threading.Tasks;
using System;

using Xamarin.Forms;

using PayMe.Apps.Data;
using PayMe.Apps.Resources;
using PayMe.Apps.Views;
using PayMe.Apps.Helpers;

namespace PayMe.Apps.ViewModels
{
    public abstract class ViewManagementPage<T> : ContentPage where T : BaseSyncEntity
    {

        public ObservableCollection<T> Items { get; set; } = new ObservableCollection<T>();

        protected abstract Task ToolbarItemNewEntity_ClickedAsync(object sender, EventArgs e);

        protected abstract Task ItemTappedForEdition_ClickedAsync(object sender, T item);

        public ViewManagementPage()
        {
            DataStore = PayMeDataStore.DefaultDataStore;

            SyncIndicator = ViewElementCustomSetting.GetDefaultActivityIndicator();
            ItemListView = new ListView
            {
                IsPullToRefreshEnabled = true
            };
            
            ItemListView.Refreshing += OnRefresh;
            ItemListView.ItemsSource = Items;

            MainBaseLayout = new StackLayout {
                Padding = Helpers.ViewElementCustomSetting.GetStandardThickness(),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    SyncIndicator,
                    ItemListView
                }
            };
            Content = MainBaseLayout;

            var newEntityToolbarItem = new ToolbarItem
            {
                Icon = Device.OnPlatform(null, "add.png", null)
            };
            newEntityToolbarItem.Clicked += async (sender, args) =>
            {
                await ToolbarItemNewEntity_ClickedAsync(sender, args);
            };

            ToolbarItems.Add(newEntityToolbarItem);
        }

        protected virtual async Task ResfreshDataAsync(bool doSync = false)
        {
            var dataSyncCode = await Task.Run(async () =>
            {
                Items.Clear();
                var collectionRequest = await DataStore.GetItemsAsync<T>(doSync);
                foreach (var item in collectionRequest.Items)
                {
                    Items.Add(item);
                }

                return collectionRequest.Code;
            });

            if (dataSyncCode == Models.DataStoreSyncCode.NotAuthenticated)
            {
                var isAccept = await DisplayAlert(Strings.Message_Warning_WaitTitle, 
                                            Strings.Message_Profile_SyncRequiresSignin, Strings.Label_Yes, Strings.Label_No);
                if (isAccept)
                {
                    var page = new ProfilePage(true);
                    await Navigation.PushModalAsync(page, true);
                }
            }
        }

        protected virtual async void OnRefresh(object sender, EventArgs e)
        {
            var list = sender as ListView;
            Exception error = null;

            try
            {
                await ResfreshDataAsync(true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert(Strings.Label_Error_SyncError_Title, $"Couldn't refresh data ({error.Message}).", Strings.Label_Okay);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResfreshDataAsync(false).ConfigureAwait(false);
        }

        protected internal readonly PayMeDataStore DataStore;

        protected internal ActivityIndicator SyncIndicator;
        protected internal ListView ItemListView;
        protected internal StackLayout MainBaseLayout;

    }
}