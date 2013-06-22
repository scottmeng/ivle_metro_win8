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
        private List<DataStructure.Module> modules;
        private List<DataStructure.Announcement> recentAnnouncements;
        private List<DataStructure.Class> classes;
        private Color[] moduleColors;

        public MainPage()
        {
            this.InitializeComponent();

            modules = new List<DataStructure.Module>();
            recentAnnouncements = new List<DataStructure.Announcement>();
            classes = new List<DataStructure.Class>();


            // to be changed
            moduleColors = new Color[10];
            moduleColors[0] = Color.FromArgb(255, 162, 0, 255);
            moduleColors[1] = Color.FromArgb(255, 255, 0, 151);
            moduleColors[2] = Color.FromArgb(255, 0, 171, 169);
            moduleColors[3] = Color.FromArgb(255, 140, 191, 38);
            moduleColors[4] = Color.FromArgb(255, 160, 80, 0);
            moduleColors[5] = Color.FromArgb(255, 230, 113, 184);
            moduleColors[6] = Color.FromArgb(255, 240, 150, 9);
            moduleColors[7] = Color.FromArgb(255, 27, 161, 226); 
            moduleColors[8] = Color.FromArgb(255, 229, 20, 0);
            moduleColors[9] = Color.FromArgb(255, 51, 153, 51);
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
            int iterator = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Duration", "0");
            parameters.Add("IncludeAllInfo", "true");

            String modulesResponse = await Utils.RequestSender.GetResponseString("Modules", parameters);
            DataStructure.ModuleInfoWrapper moduleWrapper = JsonConvert.DeserializeObject<DataStructure.ModuleInfoWrapper>(modulesResponse);

            if (moduleWrapper.comments.Equals("Valid login!"))
            {
                foreach (DataStructure.Module module in moduleWrapper.modules)
                {
                    foreach (DataStructure.Announcement announcement in module.moduleAnnouncements)
                    {
                        announcement.GenerateDisplayContent(module.moduleCode);
                        this.recentAnnouncements.Add(announcement);
                    }
                    module.SetModuleColor(moduleColors[iterator]);

                    this.modules.Add(module);
                    iterator++;
                }
            }

            moduleListView.ItemsSource = modules;

            announcementListView.ItemsSource = recentAnnouncements;
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
    }
}
