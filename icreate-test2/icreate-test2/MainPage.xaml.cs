using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : icreate_test2.Common.LayoutAwarePage
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            
            // cache the page for future usage
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
       
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
            Utils.DataManager.SortAnnouncementWrtTime();

            moduleGridView.Source = Utils.DataManager.GetModules();
            newAnnouncementListView.Source = Utils.DataManager.GetAnnouncements();

            dailyListView.Source = Utils.DataManager.GetClasses();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void prevDay(object sender, RoutedEventArgs e)
        {

        }
        private void nextDay(object sender, RoutedEventArgs e)
        {

        }

        private void Logoff_Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.TokenManager.RemoveToken();
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void AnnoucementListViewTapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Announcement selectedAnnouncement = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Announcement;

            int moduleIndex = Utils.DataManager.GetModuleIndexByModuleCode(selectedAnnouncement.announceModuleId);
            int announcementIndex = Utils.DataManager.GetAnnouncementIndex(moduleIndex, selectedAnnouncement.announceID);
            
            DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, announcementIndex);

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ItemPage), navParams);
            }
        }

        private void ModuleTileTapped(object sender, TappedRoutedEventArgs e)
        {
            // find the selected module and get the index of this module in module list
            // pass the index of the selected module as a parameter to module page
            DataStructure.Module selectedModule = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Module;

            int moduleIndex = Utils.DataManager.GetModuleIndex(selectedModule);

            DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, -1);

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ItemPage), navParams);
            }
        }

        internal void ProcessQueryText(string p)
        {
            throw new NotImplementedException();
        }
    }

   
}
