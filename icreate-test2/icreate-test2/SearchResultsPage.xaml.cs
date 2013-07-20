using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Search;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace icreate_test2
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : icreate_test2.Common.LayoutAwarePage
    {
        SearchPane searchPane;
        string searchString;

        public SearchResultsPage()
        {
            this.InitializeComponent();

            searchPane = SearchPane.GetForCurrentView();
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            searchString = (navigationParameter as String).ToLower();

            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       Only the first filter, typically "All", should pass true as a third argument in
            //       order to start in an active state.  Results for the active filter are provided
            //       in Filter_SelectionChanged below.

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // Communicate results through the view model
            this.DefaultViewModel["QueryText"] = '\u201c' + searchString + '\u201d';
            this.DefaultViewModel["Filters"] = filterList;
            this.DefaultViewModel["ShowFilters"] = filterList.Count > 1;
        }

        /// <summary>
        /// Invoked when a filter is selected using the ComboBox in snapped view state.
        /// </summary>
        /// <param name="sender">The ComboBox instance.</param>
        /// <param name="e">Event data describing how the selected filter was changed.</param>
        void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Determine what filter was selected
            var selectedFilter = e.AddedItems.FirstOrDefault() as Filter;
            if (selectedFilter != null)
            {
                // Mirror the results into the corresponding Filter object to allow the
                // RadioButton representation used when not snapped to reflect the change
                selectedFilter.Active = true;

                IEnumerable<DataStructure.Searchable> searchResults = from result in Utils.DataManager.searchables
                                                                        where result.resultTitle.ToLower().Contains(searchString) ||
                                                                              result.resultContent.ToLower().Contains(searchString)
                                                                        orderby result.resultTitle ascending
                                                                        select result;

                this.DefaultViewModel["Results"] = searchResults;

                // TODO: Respond to the change in active filter by setting this.DefaultViewModel["Results"]
                //       to a collection of items with bindable Image, Title, Subtitle, and Description properties

                // Ensure results are found
                object results;
                IEnumerable<DataStructure.Searchable> resultsCollection;

                if (this.DefaultViewModel.TryGetValue("Results", out results) &&
                    (resultsCollection = results as IEnumerable<DataStructure.Searchable>) != null &&
                    resultsCollection.Count() != 0)
                {
                    VisualStateManager.GoToState(this, "ResultsFound", true);
                    return;
                }
            }

            // Display informational text when there are no search results.
            VisualStateManager.GoToState(this, "NoResultsFound", true);
        }

        /// <summary>
        /// Invoked when a filter is selected using a RadioButton when not snapped.
        /// </summary>
        /// <param name="sender">The selected RadioButton instance.</param>
        /// <param name="e">Event data describing how the RadioButton was selected.</param>
        void Filter_Checked(object sender, RoutedEventArgs e)
        {
            // Mirror the change into the CollectionViewSource used by the corresponding ComboBox
            // to ensure that the change is reflected when snapped
            if (filtersViewSource.View != null)
            {
                var filter = (sender as FrameworkElement).DataContext;
                filtersViewSource.View.MoveCurrentTo(filter);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            searchPane.SuggestionsRequested += searchPane_SuggestionsRequested;
            searchPane.ShowOnKeyboardInput = true;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            searchPane.SuggestionsRequested -= searchPane_SuggestionsRequested;
            searchPane.ShowOnKeyboardInput = false;
        }

        public static void searchPane_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            args.Request.SearchSuggestionCollection.AppendQuerySuggestions((from result in Utils.DataManager.searchables
                                                                            where result.resultTitle.ToLower().StartsWith(args.QueryText.ToLower())
                                                                            orderby result.resultTitle ascending
                                                                            select result.resultTitle).Take(5));
        }

        /// <summary>
        /// View model describing one of the filters available for viewing search results.
        /// </summary>
        private sealed class Filter : icreate_test2.Common.BindableBase
        {
            private String _name;
            private int _count;
            private bool _active;

            public Filter(String name, int count, bool active = false)
            {
                this.Name = name;
                this.Count = count;
                this.Active = active;
            }

            public override String ToString()
            {
                return Description;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get { return _active; }
                set { this.SetProperty(ref _active, value); }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }
        }

        private void GoBack_click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SearchResultTapped(object sender, ItemClickEventArgs e)
        {
            DataStructure.Searchable selectedResult = e.ClickedItem as DataStructure.Searchable;

            if (selectedResult != null)
            {
                if (selectedResult.announcementId != null)
                {
                    // this is an announcement
                    int moduleIndex = Utils.DataManager.GetModuleIndexByModuleId(selectedResult.moduleId);
                    int announcementIndex = Utils.DataManager.GetAnnouncementIndex(moduleIndex, selectedResult.announcementId);

                    DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, announcementIndex);

                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(ItemPage), navParams);
                    }

                }
                else if (selectedResult.moduleId != null)
                {
                    // this is a module
                    int moduleIndex = Utils.DataManager.GetModuleIndexByModuleId(selectedResult.moduleId);
                    DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, -1);

                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(ItemPage), navParams);
                    }
                }
            }
        }
    }
}
