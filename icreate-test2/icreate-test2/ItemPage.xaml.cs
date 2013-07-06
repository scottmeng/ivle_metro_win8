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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ItemPage : icreate_test2.Common.LayoutAwarePage
    {
        private MediaExtensionManager extensions = new MediaExtensionManager();

        // parameters passed from main page
        private int _moduleIndex;
        private int _announcementIndex;

        private int _currentForumIndex;
        
        private DataStructure.Module _currentModule;
        private List<DataStructure.Module> _otherModules;
        private List<DataStructure.Workbin> _workbins;
        private List<DataStructure.Folder> _currentFolders;
        private List<DataStructure.File> _currentFiles;

        // store parent/child folders in hierachy 
        private List<DataStructure.Folder> _folderTree;

        public ItemPage()
        {
            this.InitializeComponent();

            _folderTree = new List<DataStructure.Folder>();
            _workbins = new List<DataStructure.Workbin>();
            _otherModules = new List<DataStructure.Module>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                DataStructure.NavParams navParams = e.Parameter as DataStructure.NavParams;

                _moduleIndex = navParams.moduleIndex;
                _announcementIndex = navParams.announcementIndex;

                _currentModule = Utils.DataManager.GetModuleAt(_moduleIndex);

                mainModuleName.Text = _currentModule.moduleCode;

                foreach (DataStructure.Module module in Utils.DataManager.GetModules())
                {
                    if (module.moduleId != _currentModule.moduleId)
                    {
                        _otherModules.Add(module);
                    }
                }
            }

            await GetExamAsync();

            _currentModule.GenerateModuleItemList();

            base.OnNavigatedTo(e);

            await GetWorkbinAsync();
            await GetForumAsync();
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
                moduleAcadYear_textblock.Text = _currentModule.moduleAcadYear + _currentModule.moduleSemester;
                moduleMc_textblock.Text = _currentModule.moduleMc;
                moduleExamtime_textblock.Text = _currentModule.moduleExamInfos[0].examInfo;
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

            // create forum title for display
            if (_currentModule.isForumAvailable)
            {

            }

            // if announcement index passed from main page is valid
            if (_announcementIndex >= 0)
            {
                itemList.SelectedIndex = 1;
                flipView.SelectedIndex = 1;

                announcementListView.SelectedIndex = _announcementIndex;
            }
            else
            {
                itemList.SelectedIndex = 0;
                flipView.SelectedIndex = 0;
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

            // make sure the tap event occurs on list items
            if (selectedItem != null)
            {
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
                        foreach (DataStructure.Forum forum in _currentModule.moduleForums)
                        {
                            forum.GenerateAllTitles();
                        }
                        foreach (DataStructure.Heading heading in _currentModule.moduleForums[_currentForumIndex].forumHeadings)
                        {
                            foreach (DataStructure.Thread thread in heading.headingThreads)
                            {
                                thread.GenerateAllThread();
                            }
                        }
                        flipView.SelectedIndex = 4;
                        headers.Source = _currentModule.moduleForums[selectedItem.itemIndex].forumAllTitles;

                        break;
                    default:
                        break;
                }
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

        private async Task GetExamAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);

            String examResponse = await Utils.RequestSender.GetResponseStringAsync("Timetable_ModuleExam", parameters);
            DataStructure.ExamInfoWrapper examInfoWrapper = JsonConvert.DeserializeObject<DataStructure.ExamInfoWrapper>(examResponse);

            _currentModule.moduleExamInfos = examInfoWrapper.examInfos;
        }

        private void onFolderSelected(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Folder selectedFolder = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Folder;

            _currentFolders = selectedFolder.folderInnerFolders;
            _currentFiles = selectedFolder.folderFiles;

            folder.Source = _currentFolders;
            file.Source = _currentFiles;

            _folderTree.Add(selectedFolder);
        }
                
        private void menuButtonClick(object sender, RoutedEventArgs e)
        {
            Flyout f = new Flyout();

            f.Placement = PlacementMode.Top;
            f.PlacementTarget = menuButton; // this is an UI element (usually the sender)

            Menu m = new Menu();

            for (int i = 0; i < _otherModules.Count; i++)
            {
                MenuItem mi = new MenuItem();
                mi.Text = (_otherModules[i]).moduleCode + " " + (_otherModules[i]).moduleName;
                mi.Tag = _otherModules[i].moduleId;
                mi.Tapped += otherModule_Tapped;
                m.Items.Add(mi);
            }
            f.Content = m;
            f.IsOpen = true;
        }

        private void otherModule_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuItem selectedModule = sender as MenuItem;

            string selectedModuleId = selectedModule.Tag as string;

            int moduleIndex = Utils.DataManager.GetModuleIndexByModuleId(selectedModule.Tag as string);

            DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, -1);

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ItemPage), navParams);
            }
        }

        private async void onFileSelected(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.File selectedFile = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.File;


            HttpClient client = new HttpClient();

            // http get request to validate token
            HttpResponseMessage response = await client.GetAsync(Utils.LAPI.GenerateDownloadURL(selectedFile.fileId));

            // make sure the http reponse is successful
            response.EnsureSuccessStatusCode();

            // open (or create if non-existing) the base folder under documents library
            StorageFolder appFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("IVLE_Metro", CreationCollisionOption.OpenIfExists);
            
            // open/create module folder
            String moduleFolderName = _currentModule.moduleCode.Replace("/", "_");
            StorageFolder currentFolder = await appFolder.CreateFolderAsync(moduleFolderName, CreationCollisionOption.OpenIfExists);

            
            foreach (DataStructure.Folder folder in _folderTree)
            {
                currentFolder = await currentFolder.CreateFolderAsync(folder.folderName, CreationCollisionOption.OpenIfExists);
            }

            // create the file
            StorageFile storageFile = await currentFolder.CreateFileAsync(selectedFile.fileName, CreationCollisionOption.OpenIfExists);

            // store data in the file
            using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
            using (Stream inputStream = await response.Content.ReadAsStreamAsync())
            {
                inputStream.CopyTo(outputStream);
            }

            // if file is sucessfully created and stored
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
            DataStructure.PostTitle selectedPostTitle = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.PostTitle;

            if (selectedPostTitle != null && !selectedPostTitle.isPostHeading)
            {
                foreach (DataStructure.Heading heading in _currentModule.moduleForums[_currentForumIndex].forumHeadings)
                {
                    foreach (DataStructure.Thread thread in heading.headingThreads)
                    {
                        if (thread.threadId == selectedPostTitle.threadId)
                        {
                            threads.Source = thread.threadAllThreads;
                        }
                    }
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool _value = (bool)value;
            if (_value)
                return "#FF00FF";
            else
                return "#00FF00";
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool _value = (bool)value;
            if (_value)
                return "0,0,0,0";
            else
                return "20,0,0,0";
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
