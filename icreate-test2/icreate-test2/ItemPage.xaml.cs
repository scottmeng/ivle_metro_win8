using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Search;
using System.ComponentModel;
using System.Collections.ObjectModel;

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

        private int _currentForumIndex = 0;
        private bool _isToPostNewThread = false;
        private string _headingId;
        private bool isRightClicking;
        private DataStructure.Module _currentModule;
        private List<DataStructure.Module> _otherModules;
        private DataStructure.ModuleItem _currentItem;
        private DataStructure.Workbin _currentWorkbin;
        private DataStructure.Folder _currentFolder;
        private List<DataStructure.Grade> _allGrades;
        private DataStructure.Thread _currentThread;
        private DataStructure.Thread _currentBaseThread;

        // store parent/child folders in hierachy 
        private List<DataStructure.Folder> _folderTree;

        private int _offSet = 0;

        public ItemPage()
        {
            this.InitializeComponent();

            _folderTree = new List<DataStructure.Folder>();
            _otherModules = new List<DataStructure.Module>();
            _allGrades = new List<DataStructure.Grade>();

            // virtual keyboard handler
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Showing += (s, args) =>
            {
                _offSet = (int)args.OccludedRect.Height;
                args.EnsuredFocusedElementInView = true;
                var trans = new TranslateTransform();
                trans.Y = -_offSet;
                this.RenderTransform = trans;
            };

            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Hiding += (s, args) =>
            {
                var trans = new TranslateTransform();
                trans.Y = 0;
                this.RenderTransform = trans;
                args.EnsuredFocusedElementInView = false;
            };

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

                foreach (DataStructure.Module module in Utils.DataManager.modules)
                {
                    if (module.moduleId != _currentModule.moduleId)
                    {
                        _otherModules.Add(module);
                    }
                }
            }

            _currentModule.GenerateModuleItemList();

            base.OnNavigatedTo(e);

            /*
            await GetExamAsync();
            await GetWebcastAsync();
            await GetForumAsync();
            await GetWorkbinAsync();
             * */
            
            await Task.WhenAll(GetExamAsync(), GetWorkbinAsync(), GetForumAsync(), GetWebcastAsync());
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
            workbinListView.Source = _currentModule.moduleWorkbins;
            ObservableCollection<DataStructure.ModuleItem> moduleItems = _currentModule.moduleItems;
            for (int i = 0; i < moduleItems.Count; i++)
            {
                moduleItems[i].itemShowColor = moduleItems[i].itemPrimaryColor;
            }

            try
            {
                // display module information
                moduleName_textblock.Text = _currentModule.moduleName;
                moduleCode_textblock.Text = _currentModule.moduleCode;
                moduleAcadYear_textblock.Text = _currentModule.moduleAcadYear + _currentModule.moduleSemester;
                moduleMc_textblock.Text = _currentModule.moduleMc;
                lecturerListView.Source = _currentModule.moduleLecturers;
            }
            catch
            {
            }

            // display gradebook
            if(_currentModule.isGradebookAvailable)
            {
                foreach (DataStructure.Gradebook gradebook in _currentModule.moduleGradebooks)
                {
                    gradebook.GenerateGradebookCategoryDisplay();
                    _allGrades = _allGrades.Concat<DataStructure.Grade>(gradebook.gradebookGrades).ToList<DataStructure.Grade>();
                }

                grades.Source = _allGrades;
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
                //itemList.SelectedIndex = 1;
                flipView.SelectedItem = null;
                flipView.SelectedIndex = 1;
                var obj = flipView.SelectedValue;
                _currentItem = _currentModule.moduleItems[1];
                moduleItems[1].itemShowColor = gray;

                //announcementListView.SelectedIndex = _announcementIndex;
            }
            else
            {
                //itemList.SelectedIndex = 0;
                moduleItems[0].itemShowColor = gray;
                flipView.SelectedIndex = 0;
                _currentItem = _currentModule.moduleItems[0];
            }
            SolidColorBrush backgroundBrush = new SolidColorBrush(_currentModule.modulePrimaryColor);
            leftStackPanel.Background = backgroundBrush;

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

        private async Task GetWebcastAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("TitleOnly", "false");

            string webcastsResponse = await Utils.RequestSender.GetResponseStringAsync("Webcasts", parameters);

            if (webcastsResponse != null)
            {
                DataStructure.WebcastWrapper webcastWrapper = JsonConvert.DeserializeObject<DataStructure.WebcastWrapper>(webcastsResponse);

                _currentModule.moduleWebcasts = webcastWrapper.webcasts;
            }
            else
            {
                //MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                //await noInternetDialog.ShowAsync();
            }
        }

        private async Task GetWorkbinAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("TitleOnly", "false");

            string workbinsResponse = await Utils.RequestSender.GetResponseStringAsync("Workbins", parameters);

            if (workbinsResponse != null)
            {
                DataStructure.WorkbinWrapper workbinWrapper = JsonConvert.DeserializeObject<DataStructure.WorkbinWrapper>(workbinsResponse);

                _currentModule.moduleWorkbins = workbinWrapper.workbins;
            }
            else
            {
                //MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                //await noInternetDialog.ShowAsync();
            }
        }

        private async Task GetForumAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);
            parameters.Add("Duration", "0");
            parameters.Add("IncludeThreads", "true");

            String forumsResponse = await Utils.RequestSender.GetResponseStringAsync("Forums", parameters);

            if (forumsResponse != null)
            {
                DataStructure.ForumWrapper forumWrapper = JsonConvert.DeserializeObject<DataStructure.ForumWrapper>(forumsResponse);

                foreach (DataStructure.Forum forum in forumWrapper.forums)
                {
                    foreach (DataStructure.Forum mForum in _currentModule.moduleForums)
                    {
                        if (forum.forumId == mForum.forumId)
                        {
                            mForum.forumHeadings = forum.forumHeadings;
                        }
                    }
                }

                if (_currentModule.isForumAvailable)
                {
                    updateForum();
                }
            }
            else
            {
                //MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                //await noInternetDialog.ShowAsync();
            }
        }

        private async Task GetExamAsync()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseID", _currentModule.moduleId);

            String examResponse = await Utils.RequestSender.GetResponseStringAsync("Timetable_ModuleExam", parameters);

            if (examResponse != null)
            {
                DataStructure.ExamInfoWrapper examInfoWrapper = JsonConvert.DeserializeObject<DataStructure.ExamInfoWrapper>(examResponse);

                _currentModule.moduleExamInfos = examInfoWrapper.examInfos;

                try
                {
                    moduleExamtime_textblock.Text = _currentModule.moduleExamInfos[0].examInfo;
                }
                catch { }
            }
            else
            {
                //MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                //await noInternetDialog.ShowAsync();
            }
        }

        private void onFolderSelected(object sender, TappedRoutedEventArgs e)
        {
            if (_currentFolder != null)
            {
                _folderTree.Add(_currentFolder);
            }

            _currentFolder = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Folder;

            this.folder.Source = _currentFolder.folderInnerFolders;
            this.file.Source = _currentFolder.folderFiles;

            upFolderButton.Visibility = Visibility.Visible;
            upFolderButton_snapped.Visibility = Visibility.Visible;
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
            StorageFile targetFile;

            DataStructure.File selectedFile = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.File;

            // check if token exists
            Windows.Storage.ApplicationDataContainer folderTokens = Windows.Storage.ApplicationData.Current.LocalSettings;
            string listToken = (string) folderTokens.Values[_currentModule.moduleId];

            IStorageFolder moduleBaseFolder;

            // if token exists
            // get folder access
            // otherwise create folder under Downloads library
            if(listToken != null)
            {
                try
                {
                    moduleBaseFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(listToken);
                }
                catch
                {
                    moduleBaseFolder = null;
                }

                if (moduleBaseFolder == null)
                {
                    String moduleFolderName = _currentModule.moduleCode.Replace("/", "_");

                    moduleBaseFolder = await Windows.Storage.DownloadsFolder.CreateFolderAsync(moduleFolderName, CreationCollisionOption.GenerateUniqueName);
                    listToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(moduleBaseFolder, moduleBaseFolder.Name);

                    folderTokens.Values[_currentModule.moduleId] = listToken;
                }
            }
            else
            {
                String moduleFolderName = _currentModule.moduleCode.Replace("/", "_");

                moduleBaseFolder = await Windows.Storage.DownloadsFolder.CreateFolderAsync(moduleFolderName, CreationCollisionOption.GenerateUniqueName);
                listToken = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(moduleBaseFolder, moduleBaseFolder.Name);

                folderTokens.Values[_currentModule.moduleId] = listToken;
            }


            HttpClient client = new HttpClient();

            // http get request to validate token
            HttpResponseMessage response = await client.GetAsync(Utils.LAPI.GenerateDownloadURL(selectedFile.fileId));

            // make sure the http reponse is successful
            response.EnsureSuccessStatusCode();

            IStorageFolder currentFolder = moduleBaseFolder; 

            foreach (DataStructure.Folder folder in _folderTree)
            {
                currentFolder = await currentFolder.CreateFolderAsync(folder.folderName, CreationCollisionOption.OpenIfExists);
            }

            try
            {
                targetFile = await currentFolder.GetFileAsync(selectedFile.fileName);
            }
            catch
            {
                targetFile = null;
            }
            // if file has not been downloaded before
            // download it
            if (targetFile == null)
            {
                // create the file
                targetFile = await currentFolder.CreateFileAsync(selectedFile.fileName, CreationCollisionOption.OpenIfExists);

                // store data in the file
                using (Stream outputStream = await targetFile.OpenStreamForWriteAsync())
                using (Stream inputStream = await response.Content.ReadAsStreamAsync())
                {
                    inputStream.CopyTo(outputStream);
                }
            }
            // if file does exist
            // check the creation date and the file upload time
            // if creation date is prior to upload date, there must have been
            // a newer version of the file
            else if (targetFile.DateCreated.CompareTo(selectedFile.fileUploadTime) < 0)
            {
                // store data in the file
                using (Stream outputStream = await targetFile.OpenStreamForWriteAsync())
                using (Stream inputStream = await response.Content.ReadAsStreamAsync())
                {
                    inputStream.CopyTo(outputStream);
                }
            }

            // if file is sucessfully created and stored
            if (targetFile != null)
            {
                // Set the option to show the picker
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = false;

                // Launch the retrieved file
                await Windows.System.Launcher.LaunchFileAsync(targetFile, options);
            }
            else
            {
                // Could not find file
            }
        }

        private void thread_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.PostTitle selectedPostTitle = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.PostTitle;

            if (selectedPostTitle != null)
            {
                if (selectedPostTitle.isPostHeading)
                {
                    _isToPostNewThread = true;
                    _headingId = selectedPostTitle.headingId;
                    replyStackPanel.Visibility = Visibility.Visible;
                    titleTextBox.Text = "";
                    contentTextBox.Text = "";
                }
                else
                {
                    // openning an existing thread
                    foreach (DataStructure.Heading heading in _currentModule.moduleForums[_currentForumIndex].forumHeadings)
                    {
                        foreach (DataStructure.Thread thread in heading.headingThreads)
                        {
                            if (thread.threadId == selectedPostTitle.threadId)
                            {
                                _currentBaseThread = thread;
                                threads.Source = _currentBaseThread.threadAllThreads;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        // generate forum post titles and all sub-threads
        private void updateForum()
        {
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
        }

        private void onUpButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_folderTree.Count > 0)
            {
                _currentFolder = _folderTree.Last<DataStructure.Folder>();

                folder.Source = _currentFolder.folderInnerFolders;
                file.Source = _currentFolder.folderFiles;

                _folderTree.RemoveAt(_folderTree.Count - 1);
            }
            else
            {
                _currentFolder = null;

                folder.Source = _currentWorkbin.workbinFolders;
                file.Source = new List<DataStructure.File>();
                upFolderButton.Visibility = Visibility.Collapsed;
                upFolderButton_snapped.Visibility = Visibility.Collapsed;
            }
        }

        private void innerThread_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Thread tappedThread = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Thread;

            if (tappedThread != null)
            {
                if (_currentThread != null && tappedThread.threadId == _currentThread.threadId && replyStackPanel.Visibility == Visibility.Visible)
                {
                    replyStackPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _isToPostNewThread = false;
                    _currentThread = tappedThread;
                    replyStackPanel.Visibility = Visibility.Visible;
                    titleTextBox.Text = "Re: " + tappedThread.threadTitle;
                    contentTextBox.Text = "";
                }
            }
        }

        private async void PostNewThread()
        {
            string title = titleTextBox.Text;
            string content = contentTextBox.Text;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("HeadingID", _headingId);
            parameters.Add("Title", title);
            parameters.Add("Reply", content);

            string state = await Utils.RequestSender.SendHttpPostRequestAsync("Forum_PostNewThread_JSON", parameters);

            // if there is valid reply
            // close reply textbox
            if (state != null)
            {
                replyStackPanel.Visibility = Visibility.Collapsed;

                // refresh the heading list
                await GetForumAsync();

                // generate content for display
                updateForum();
            }
            else
            {
                // TODO
                // display error message
            }
        }

        private async void ReplyThread()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ThreadID", _currentThread.threadId);
            parameters.Add("Title", titleTextBox.Text);
            parameters.Add("Reply", contentTextBox.Text);

            string state = await Utils.RequestSender.SendHttpPostRequestAsync("Forum_ReplyThread_JSON", parameters);

            // if there is valid reply
            // close reply textbox
            if (state != null)
            {
                replyStackPanel.Visibility = Visibility.Collapsed;

                // TO-DO ]
                parameters.Clear();
                parameters.Add("ThreadID", _currentBaseThread.threadId);
                parameters.Add("Duration", "0");
                parameters.Add("GetSubThreads", "true");

                state = await Utils.RequestSender.GetResponseStringAsync("Forum_Threads", parameters);

                if (state != null)
                {
                    DataStructure.ThreadWrapper threadWrapper = JsonConvert.DeserializeObject<DataStructure.ThreadWrapper>(state);
                    _currentBaseThread.threadInnerThreads = threadWrapper.threads[0].threadInnerThreads;
                    _currentBaseThread.GenerateAllThread();
                }
                else
                {
                    // display error message
                    MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                    await noInternetDialog.ShowAsync();
                }
            }
            else
            {
                // display error message
                MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                await noInternetDialog.ShowAsync();
            }
            
        }

        private void replyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isToPostNewThread)
            {
                PostNewThread();
            }
            else
            {
                ReplyThread();
            }
        }

        private void FolderPointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void FolderButtonEntered(object sender, PointerRoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255,211,211,211));
            SolidColorBrush brushBlue = new SolidColorBrush(Color.FromArgb(255,0,0,255));
            FolderGrid.Background = brush;
            FolderGrid_snapped.Background = brush;
        }

        private void FolderButtonExited(object sender, PointerRoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            FolderGrid.Background = brush;
            FolderGrid_snapped.Background.Opacity = 0;
        }

        private void mainGrid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            // if the event is not triggerd from a textbox
            // and the key pressed is backspace
            // then navigate back to the previous page
            if (e.Key == Windows.System.VirtualKey.Back)
            {
                GoBack(sender, e);
            }
        }

        private void titleTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void contentTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private async void lecturesListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Lecturer selectedLecturer = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Lecturer;

            if (selectedLecturer != null)
            {
                var uri = new Uri("mailto:" + selectedLecturer.lecturerMember.memberEmail);
                // Launch the URI.
                bool success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
        }
        Color gray = Color.FromArgb(255, 96, 96, 96);
        private void itemListEntered(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.ModuleItem selectedModuleItem = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.ModuleItem;
            
            if (selectedModuleItem != null)
            {
                if(selectedModuleItem.itemShowColor != gray)
                    selectedModuleItem.itemShowColor = selectedModuleItem.itemSecondaryColor;
                if(isRightClicking)
                    selectedModuleItem.itemShowColor = gray;
            }
        }

        private void itemListExited(object sender, PointerRoutedEventArgs e)
        {
            ObservableCollection<DataStructure.ModuleItem> moduleItems = _currentModule.moduleItems;
            for (int i = 0; i < moduleItems.Count; i++)
            {
                if (moduleItems[i].itemShowColor != gray)
                    moduleItems[i].itemShowColor = moduleItems[i].itemPrimaryColor;
            }
        }

        private void itemListPressed(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.ModuleItem selectedModuleItem = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.ModuleItem;
            ObservableCollection<DataStructure.ModuleItem> moduleItems = _currentModule.moduleItems;
            isRightClicking = true;

            for (int i = 0; i < moduleItems.Count; i++)
            {
                moduleItems[i].itemShowColor = moduleItems[i].itemPrimaryColor;
            }
            
            if (selectedModuleItem != null)
            {
                selectedModuleItem.itemShowColor = gray;
            }
        }


        private void itemListReleased(object sender, PointerRoutedEventArgs e)
        {
            isRightClicking = false;
            DataStructure.ModuleItem selectedItem = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.ModuleItem;

            // make sure the tap event occurs on list items
            if (selectedItem != null && selectedItem != _currentItem)
            {
                switch (selectedItem.itemType)
                {
                    case DataStructure.ItemType.ANNOUNCEMENT:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 1;
                        break;

                    case DataStructure.ItemType.GRADEBOOK:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 3;
                        break;

                    case DataStructure.ItemType.ABOUT:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 0;
                        break;

                    case DataStructure.ItemType.WEBCAST:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 5;

                        // generate a complete list of video files
                        // and store under webcast object
                        _currentModule.moduleWebcasts[selectedItem.itemIndex].GenerateVideoFileList();
                        webcastGridViews.Source = _currentModule.moduleWebcasts[selectedItem.itemIndex].webcastAllVideoFiles;
                        playerStackpanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        webcastGridView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;

                    case DataStructure.ItemType.WORKBIN:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 2;

                        _currentWorkbin = _currentModule.moduleWorkbins[selectedItem.itemIndex];

                        folder.Source = _currentWorkbin.workbinFolders;
                        file.Source = new List<DataStructure.File>();

                        _folderTree.Clear();
                        upFolderButton.Visibility = Visibility.Collapsed;
                        upFolderButton_snapped.Visibility = Visibility.Collapsed;

                        break;

                    case DataStructure.ItemType.FORUM:
                        flipView.SelectedItem = null;
                        flipView.SelectedIndex = 4;

                        _currentForumIndex = selectedItem.itemIndex;

                        // generate content for display
                        updateForum();

                        headers.Source = _currentModule.moduleForums[selectedItem.itemIndex].forumAllTitles;
                   
                        break;

                    default:
                        break;
                }

                _currentItem = selectedItem;
            }
        }

        private void workbinListTapped(object sender, TappedRoutedEventArgs e)
        {
            _currentWorkbin = _currentModule.moduleWorkbins[workbinList_snapped.SelectedIndex];

            folder.Source = _currentWorkbin.workbinFolders;
            file.Source = new List<DataStructure.File>();

            _folderTree.Clear();
            upFolderButton.Visibility = Visibility.Collapsed;
            upFolderButton_snapped.Visibility = Visibility.Collapsed;
        }

        private void videoTapped(object sender, TappedRoutedEventArgs e)
        {
           
            DataStructure.VideoFile selectedItem = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.VideoFile;
            if(selectedItem!=null)
            {
                Uri uu = new Uri(selectedItem.videoMP4);
                player.Source = uu;
            }
            playerStackpanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            webcastGridView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void videoBackClick(object sender, RoutedEventArgs e)
        {
            player.Source = null;
            playerStackpanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            webcastGridView.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
    }

    // converter for thread title background color binding
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool _value = (bool)value;
            if (_value)
                return "#2F4F4F";
            else
                return "#008080";
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    // converter for thread title margin binding
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

    // converter for thread title margin binding
    public class FileIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string fileType = (string)value;
            Uri iconLocation = new Uri("ms-appx:///Assets/General.png");

            switch (fileType)
            {
                case "docx":
                case "doc":
                    iconLocation = new Uri("ms-appx:///Assets/Word.png");
                    break;
                case "pptx":
                case "ppt":
                    iconLocation = new Uri("ms-appx:///Assets/PPT.png");
                    break;
                case "pdf":
                    iconLocation = new Uri("ms-appx:///Assets/PDF.png");
                    break;
                case "xls":
                case "xlsx":
                    iconLocation = new Uri("ms-appx:///Assets/Excel.png");
                    break;
                case "zip":
                case "rar":
                    iconLocation = new Uri("ms-appx:///Assets/ZIP.png");
                    break;
                default:
                    break;
            }

            return new Windows.UI.Xaml.Media.Imaging.BitmapImage(iconLocation);
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
