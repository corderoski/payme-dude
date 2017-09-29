using PayMe.Apps.Data.Entities;
using PayMe.Apps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using PayMe.Apps.ViewModels;

namespace PayMe.Apps.Views
{
    public class TransactionTagSelectorView : ContentPage
    {

        public TransactionTagSelectorView()
        {
            var label = new Label { Text = Strings.Label_Tags };
            var commandLabelTapped = new Command<View>(async view =>
            {
                AlternateViewMode();
            });
            label.GestureRecognizers.Add(new TapGestureRecognizer { Command = commandLabelTapped, CommandParameter = this, NumberOfTapsRequired = 1 });

            tagsEntryControl = new Editor { InputTransparent = true, Keyboard = Keyboard.Plain, Text = Strings.Label_Transaction_TagEditorInitialText };
            tagsEntryControl.TextChanged += (sender, args) =>
            {
                tagsEntryControl.Text = !string.IsNullOrEmpty(tagsJoinedString) ? tagsJoinedString : Strings.Label_Transaction_TagEditorInitialText;
                AlternateViewMode();
            };
            tagsEntryControl.GestureRecognizers.Add(new TapGestureRecognizer { Command = commandLabelTapped, CommandParameter = this, NumberOfTapsRequired = 1 });

            searchBar = new SearchBar
            {
                Placeholder = Strings.Label_TagSelector_PlaceholderHelper,
                IsVisible = false
            };
            searchBar.TextChanged += (sender, e) =>
            {
                FilterContacts(searchBar.Text);
            };
            searchBar.SearchButtonPressed += (sender, e) =>
            {
                FilterContacts(searchBar.Text);
            };
            //searchBar.Unfocused += (sender, args) => this.Unfocus();

            listView = new ListView
            {
                IsPullToRefreshEnabled = true,
                IsVisible = false
            };
            listView.ItemSelected += ListView_ItemSuggestionSelected;

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = {
                    label,

                    tagsEntryControl,

                    searchBar,
                    listView
                }
            };

            var commandWhenClicked = new Command<View>(async view =>
            {

            });
        }

        public IEnumerable<Tag> GetSelection()
        {
            return tagsHashSet;
        }

        public void AlternateViewMode()
        {
            if (modeType == ManagementPageModeType.ReadOnly) SetEditionMode();
            else SetReadOnlyMode();
        }

        private void ListView_ItemSuggestionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var item = (Tag)e.SelectedItem;

            tagsHashSet.Add(item);

            ((ListView)sender).SelectedItem = null;
        }

        private void SetReadOnlyMode()
        {
            modeType = ManagementPageModeType.ReadOnly;

            var selectedTags = GetSelection().Select(p => p.Name);
            tagsJoinedString = string.Join(",", selectedTags);

            tagsEntryControl.IsVisible = true;
            searchBar.IsVisible = false;
            listView.IsVisible = false;
        }

        private void SetEditionMode()
        {
            modeType = ManagementPageModeType.Edit;

            tagsEntryControl.IsVisible = false;
            searchBar.IsVisible = true;
            listView.IsVisible = true;
        }

        private void FilterContacts(string filter)
        {
            listView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                listView.ItemsSource = DataSource;
            }
            else
            {
                var value = filter.ToLower();
                listView.ItemsSource = DataSource.Where(x => x.Name.ToLower().Contains(value));
            }
            listView.EndRefresh();
        }

        private ManagementPageModeType modeType;

        private SearchBar searchBar;
        private ListView listView;
        private Editor tagsEntryControl;

        private string tagsJoinedString = null;
        private HashSet<Tag> tagsHashSet = new HashSet<Tag>();

        public ObservableCollection<Tag> DataSource { get; set; } = new ObservableCollection<Tag>();

    }
}