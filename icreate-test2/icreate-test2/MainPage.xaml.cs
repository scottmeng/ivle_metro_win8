using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : icreate_test2.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            List<TestListView> itemsList = new List<TestListView>();
            itemsList.Add(new TestListView() { Background = "#0FFF00", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#00FF00", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0F0F00", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0FF000", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0F0000", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#000F00", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#00F000", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0FFFF0", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0FFF0F", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FFFF00", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0FFFFF", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FFFF0F", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FFFFF0", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0FF0FF", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0F00FF", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#000FFF", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0000F0", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0F00F0", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#0000FF", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#F0F0F0", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FFF00F", Title = "cs3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });

            itemListView.ItemsSource = itemsList;

            List<TestGridView> itemsListGrid = new List<TestGridView>();
            itemsListGrid.Add(new TestGridView() { Background = "#0FFF00", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#0FFF0F", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#0FFF00", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#FFFFF0", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#FFFFF0", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#0FFF00", Title = "cs3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#FFFFF0", Title = "cs3233" });
            itemGridView.ItemsSource = itemsListGrid;

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
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public class TestListView
        {
            public string Background { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }
        public class TestGridView
        {
            public string Background { get; set; }
            public string Title { get; set; }
        }
    }
}
