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
using Callisto.Controls;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ItemPage : icreate_test2.Common.LayoutAwarePage
    {
        private MediaExtensionManager extensions = new MediaExtensionManager();
        private int _moduleNum;
        private int _moduleIndex;
        private int _announcementIndex;

        private DataStructure.Module _currentModule;
        private List<DataStructure.Module> _otherModules;
        private List<DataStructure.Workbin> _workbins;
        private List<DataStructure.Folder> _currentFolders;
        private List<DataStructure.File> _currentFiles;

        public ItemPage()
        {
            this.InitializeComponent();

            _workbins = new List<DataStructure.Workbin>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                DataStructure.NavParams navParams = e.Parameter as DataStructure.NavParams;

                _moduleIndex = navParams.moduleIndex;
                _moduleNum = Utils.DataManager.GetModules().Count;
                _announcementIndex = navParams.announcementIndex;

                _currentModule = Utils.DataManager.GetModuleAt(_moduleIndex);

                mainModuleName.Text = _currentModule.moduleCode;
                _otherModules = new List<DataStructure.Module>();
                for (int i = 0; i < _moduleNum; i++)
                {
                    if(i!=_moduleIndex)
                        _otherModules.Add(Utils.DataManager.GetModuleAt(i));
                }
            }

            await GetWorkbinAsync();
            await GetForumAsync();
            _currentModule.GenerateModuleItemList();

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
            // display tabs
            itemListView.Source = _currentModule.moduleItems;

            try
            {
                // display module information
                moduleName_textblock.Text = _currentModule.moduleName;
                moduleCode_textblock.Text = _currentModule.moduleCode;
                moduleCode_textblock.Text = _currentModule.moduleAcadYear + _currentModule.moduleSemester;
                moduleMc_textblock.Text = _currentModule.moduleMc;
            }
            catch
            {
            }

            // display gradebook
            if(_currentModule.isGradebookAvailable)
            {
                grades.Source = _currentModule.moduleGradebooks[0].gradebookGrades;
            }

            // display announcements
            if (_currentModule.isAnnouncementAvailable)
            {
                newAnnouncementListView.Source = _currentModule.moduleAnnouncements;
            }           

            // display forum
            if (_currentModule.isForumAvailable)
            {
                headers.Source = _currentModule.moduleForums[0].forumHeadings;
                if (_currentModule.moduleForums[0].forumHeadings.Length > 0)
                {
                    threads.Source = _currentModule.moduleForums[0].forumHeadings[0].headingThreads;
                    if (_currentModule.moduleForums[0].forumHeadings[0].headingThreads.Length > 0)
                    {
                        if (_currentModule.moduleForums[0].forumHeadings[0].headingThreads[0].threadInnerThreads.Length > 0)
                        {
                            innerThreads.Source = _currentModule.moduleForums[0].forumHeadings[0].headingThreads[0].threadInnerThreads;
                        }
                    }
                    else
                    {
                        innerThreads.Source = null;
                    }
                }
            }
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
                    flipView.SelectedIndex = 1;
                    break;
                case DataStructure.ItemType.GRADEBOOK:
                    flipView.SelectedIndex = 3;
                    break;
                case DataStructure.ItemType.MODULE_INFO:
                    flipView.SelectedIndex = 0;
                    break;
                case DataStructure.ItemType.WEBCAST:
                    break;
                case DataStructure.ItemType.WORKBIN:
                    flipView.SelectedIndex = 2;
                    
                    _currentFolders = _currentModule.moduleWorkbins[selectedItem.itemIndex].workbinFolders;
                    _currentFiles = new List<DataStructure.File>();

                    folder.Source = _currentFolders;
                    file.Source = _currentFiles;
                    break;
                case DataStructure.ItemType.FORUM:
                    flipView.SelectedIndex = 4;
                    break;
                default:
                    break;
            }
        }

        private async Task GetWorkbinAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("TitleOnly", "false");

            String workbinsResponse = await Utils.RequestSender.GetResponseStringAsync("Workbins", parameters);
            DataStructure.WorkbinWrapper workbinWrapper = JsonConvert.DeserializeObject<DataStructure.WorkbinWrapper>(workbinsResponse);

            _currentModule.moduleWorkbins = workbinWrapper.workbins;
        }

        private async Task GetForumAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("IncludeThreads", "true");

            String forumsResponse = await Utils.RequestSender.GetResponseStringAsync("Forums", parameters);
            DataStructure.ForumWrapper forumWrapper = JsonConvert.DeserializeObject<DataStructure.ForumWrapper>(forumsResponse);

            _currentModule.moduleForums = forumWrapper.forums;
        }

        private void onFolderSelected(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Folder selectedFolder = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Folder;

            _currentFolders = selectedFolder.folderInnerFolders;
            _currentFiles = selectedFolder.folderFiles;

            folder.Source = _currentFolders;
            file.Source = _currentFiles;
        }


        
        private void menuButtonClick(object sender, RoutedEventArgs e)
        {
            Flyout f = new Flyout();

            f.Placement = PlacementMode.Top;
            f.PlacementTarget = menuButton; // this is an UI element (usually the sender)

            Menu m = new Menu();
            MenuItem mi2 = new MenuItem();
            mi2.Text = "Another Option Here";
            for (int i = 0; i < _otherModules.Count; i++)
            {
                MenuItem mi = new MenuItem();
                mi.Text = (_otherModules[i]).moduleCode + " " + (_otherModules[i]).moduleName;
                m.Items.Add(mi);
            }
            f.Content = m;
            f.IsOpen = true;
        }


        /* TODO
         * Move this to data manager?
         * make it cleaner
         * */

        private async void onFileSelected(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.File selectedFile = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.File;


            HttpClient client = new HttpClient();

            // http get request to validate token
            HttpResponseMessage response = await client.GetAsync(Utils.LAPI.GenerateDownloadURL(selectedFile.fileId));

            // make sure the http reponse is successful
            response.EnsureSuccessStatusCode();


            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            String moduleFolderName = _currentModule.moduleCode.Replace("/", "_");
            StorageFolder moduleFolder = await storageFolder.CreateFolderAsync(moduleFolderName, CreationCollisionOption.OpenIfExists);
            StorageFile storageFile = await moduleFolder.CreateFileAsync("test.pdf", CreationCollisionOption.GenerateUniqueName);

            using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
            using (Stream inputStream = await response.Content.ReadAsStreamAsync())
            {
                inputStream.CopyTo(outputStream);
            }

            //var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("test.pdf");

            if (storageFile != null)
            {
                // Set the option to show the picker
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;

                // Launch the retrieved file
                bool success = await Windows.System.Launcher.LaunchFileAsync(storageFile, options);
                if (success)
                {
                    // File launched
                }
                else
                {
                    // File launch failed
                }
            }
            else
            {
                // Could not find file
            }
        }

        private void thread_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Thread selectedThread = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Thread;
            if (selectedThread.threadInnerThreads.Length > 0)
                innerThreads.Source = selectedThread.threadInnerThreads;
            else
                innerThreads.Source = null;
        }
    }
}
