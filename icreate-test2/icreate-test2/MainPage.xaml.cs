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

using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : icreate_test2.Common.LayoutAwarePage
    {
        private List<DataStructure.Module> modules;
        private List<DataStructure.Announcement> recentAnnouncements;
        private List<DataStructure.Class> classes;

        public MainPage()
        {
            this.InitializeComponent();

            modules = new List<DataStructure.Module>();
            recentAnnouncements = new List<DataStructure.Announcement>();
            classes = new List<DataStructure.Class>();

            /*
             * test data
             * 

            List<TestListView> itemsList = new List<TestListView>();
            itemsList.Add(new TestListView() { Background = "#00BFFF", Title = "IS3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#8A2BE2", Title = "ES3231", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#00BFFF", Title = "CG3213", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#EE0000", Title = "CS3133", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FF69B4", Title = "CS3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FF7256", Title = "CS3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#FF69B4", Title = "EC3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#006400", Title = "EE3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });
            itemsList.Add(new TestListView() { Background = "#8A2BE2", Title = "ME3233", Description = "slkdjfa;lskdjga;lsa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;laa;lskdjga;lsdkjf;ladkjf;lakdjf" });

            itemListView.ItemsSource = itemsList;

            List<TestGridView> itemsListGrid = new List<TestGridView>();
            itemsListGrid.Add(new TestGridView() { Background = "#00BFFF", Title = "IS3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#8A2BE2", Title = "ES3231" });
            itemsListGrid.Add(new TestGridView() { Background = "#FF69B4", Title = "CG3213" });
            itemsListGrid.Add(new TestGridView() { Background = "#FF7256", Title = "CS3233" });
            itemsListGrid.Add(new TestGridView() { Background = "#EE0000", Title = "EC5223" });
            itemsListGrid.Add(new TestGridView() { Background = "#006400", Title = "ME3233" });
            itemGridView.ItemsSource = itemsListGrid;
             * 
             * test data
             */

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
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Duration", "0");
            parameters.Add("IncludeAllInfo", "false");

            String modulesResponse = await Utils.RequestSender.GetResponseString("Modules", parameters);
            DataStructure.ModuleInfoWrapper moduleWrapper = JsonConvert.DeserializeObject<DataStructure.ModuleInfoWrapper>(modulesResponse);

            if (moduleWrapper.comments.Equals("Valid login!"))
            {
                foreach (DataStructure.Module module in moduleWrapper.modules)
                {
                    modules.Add(module);
                }
            }

            moduleListView.ItemsSource = modules;
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
