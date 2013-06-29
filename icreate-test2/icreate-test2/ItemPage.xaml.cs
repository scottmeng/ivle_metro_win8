using System;
using System.Net;
using System.Text;
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
using Windows.UI.Popups;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Media;

using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ItemPage : icreate_test2.Common.LayoutAwarePage
    {
        private MediaExtensionManager extensions = new MediaExtensionManager();
        private int _moduleIndex;
        private int _announcementIndex;

        private DataStructure.Module _currentModule;
        private List<DataStructure.Workbin> _workbins;

        public ItemPage()
        {
            this.InitializeComponent();

            _workbins = new List<DataStructure.Workbin>();
            extensions.RegisterByteStreamHandler("Microsoft.Media.AdaptiveStreaming.SmoothByteStreamHandler", ".ism", "text/xml");
            extensions.RegisterByteStreamHandler("Microsoft.Media.AdaptiveStreaming.SmoothByteStreamHandler", ".ism", "application/vnd.ms-sstr+xml");
            mediaElement.Source = new Uri("http://ecn.channel9.msdn.com/o9/content/smf/smoothcontent/elephantsdream/Elephants_Dream_1024-h264-st-aac.ism/manifest");
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                DataStructure.NavParams navParams = e.Parameter as DataStructure.NavParams;

                _moduleIndex = navParams.moduleIndex;
                _announcementIndex = navParams.announcementIndex;

                _currentModule = Utils.DataManager.GetModuleAt(_moduleIndex);
                _currentModule.GenerateModuleItemList();
            }

            await GetWorkbinAsync();

            base.OnNavigatedTo(e);
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
            itemListView.Source = _currentModule.moduleItems;
            newAnnouncementListView.Source = _currentModule.moduleAnnouncements;
            folder.Source = _workbins[0].workbinFolders;
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

        private void itemChanged_ListViewTapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.ModuleItem selectedItem = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.ModuleItem;

            switch (selectedItem.itemType)
            {
                case DataStructure.ItemType.ANNOUNCEMENT:
                    flipView.SelectedIndex = 0;
                    break;
                case DataStructure.ItemType.GRADEBOOK:
                    break;
                case DataStructure.ItemType.MODULE_INFO:
                    flipView.SelectedIndex = 2;
                    break;
                case DataStructure.ItemType.WEBCAST:
                    break;
                case DataStructure.ItemType.WORKBIN:
                    flipView.SelectedIndex = 1;
                    break;
                default:
                    break;
            }
            /*
            if (itemList.SelectedIndex < 2)
            {
                flipView.SelectedIndex = itemList.SelectedIndex;
            }
             * */
        }

        private async Task GetWorkbinAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("TitleOnly", "false");

            String workbinsResponse = await Utils.RequestSender.GetResponseStringAsync("Workbins", parameters);
            DataStructure.WorkbinWrapper workbinWrapper = JsonConvert.DeserializeObject<DataStructure.WorkbinWrapper>(workbinsResponse);

            foreach (DataStructure.Workbin workbin in workbinWrapper.workbins)
            {
                _workbins.Add(workbin);
            }
        }

        private void onFolderSelected(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Folder selectedFolder = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Folder;

            folder.Source = selectedFolder.folderInnerFolders;
            file.Source = selectedFolder.folderFiles;
        }
    }
}
