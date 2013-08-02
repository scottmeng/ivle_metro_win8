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
using Windows.UI.ApplicationSettings;
using Windows.ApplicationModel.Search;
using Windows.Storage;

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
        private static List<String> dayList;
        private static int todayCode;
        private static int dayListIndex;
        private bool isRightClicking;
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;

            dayListIndex = 2;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    todayCode = 4;
                    dayList = new List<string> { "Wednesday", "Thursday", "Today", "Saturday", "Monday", "Tuesday" };
                    break;
                case DayOfWeek.Monday:
                    todayCode = 0;
                    dayList = new List<string> { "Friday", "Saturday", "Today", "Tuesday", "Wednesday", "Thursday" };
                    break;
                case DayOfWeek.Tuesday:
                    todayCode = 1;
                    dayList = new List<string> { "Saturday", "Monday", "Today", "Wednesday", "Thursday", "Friday" };
                    break;
                case DayOfWeek.Wednesday:
                    todayCode = 2;
                    dayList = new List<string> { "Monday", "Tuesday", "Today", "Thursday", "Friday", "Saturday" };
                    break;
                case DayOfWeek.Thursday:
                    todayCode = 3;
                    dayList = new List<string> { "Tuesday", "Wednesday", "Today", "Friday", "Saturday", "Monday" };
                    break;
                case DayOfWeek.Saturday:
                    todayCode = 5;
                    dayList = new List<string> { "Thursday", "Friday", "Today", "Monday", "Tuesday", "Wednesday" };
                    break;
                case DayOfWeek.Sunday:
                    todayCode = 0;
                    dayList = new List<string> { "Friday", "Saturday", "Monday", "Tuesday", "Wednesday", "Thursday" };
                    break;
                default:
                    break;
            }

            // cache the page for future usage
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            //Utils.DataManager.GenerateSearchResults();

            SearchPane.GetForCurrentView().SuggestionsRequested += SearchResultsPage.searchPane_SuggestionsRequested;
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
            moduleGridView.Source = Utils.DataManager.modules;
            newAnnouncementListView.Source = Utils.DataManager.announcements;

            // let data manager populates class list for each weekday
            date_textblock.Text = dayList[dayListIndex];
            dailyListView.Source = Utils.DataManager.GetDailyClassList(todayCode);

            //initialize rightclick remember
            isRightClicking = false;
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // enable type to search
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;
            
            base.OnNavigatedTo(e);

            if (App.appState == AppState.CACHED)
            {
                // fetch module data
                await GetModulesAsync();
                // generate announcement display
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // disable type to search
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = false;

            base.OnNavigatedFrom(e);
        }

        private async Task GetModulesAsync()
        {
            int iterator = 0;
            string modulesResponse;
            loadingProgressBar.IsIndeterminate = true;
            loadingProgressBar.Visibility = Visibility.Visible;
            //loadingProgress.IsActive = true;

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Duration", "0");
            parameters.Add("IncludeAllInfo", "true");

            modulesResponse = await Utils.RequestSender.GetResponseStringAsync("Modules_Student", parameters);

            if (modulesResponse != null)
            {
                // store all data in local storage
                StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, modulesResponse);

                // deserialize data into objects
                DataStructure.ModuleInfoWrapper modulesWrapper = JsonConvert.DeserializeObject<DataStructure.ModuleInfoWrapper>(modulesResponse);

                if (modulesWrapper.comments.Equals("Valid login!"))
                {
                    // clear all announcements 
                    Utils.DataManager.ClearAnnouncement();

                    foreach (DataStructure.Module module in modulesWrapper.modules)
                    {
                        module.SetModuleColor(DataStructure.Colors.GetModuleColor(iterator), DataStructure.Colors.GetSecondaryColor(iterator));

                        foreach (DataStructure.Announcement announcement in module.moduleAnnouncements)
                        {
                            announcement.GenerateDisplayContent(module);
                            Utils.DataManager.AddAnnouncement(announcement);
                        }

                        iterator++;
                    }

                    Utils.DataManager.UpdateModules(modulesWrapper.modules);
                }

                App.appState = AppState.UPDATED;
            }

            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Visibility = Visibility.Collapsed;
            //loadingProgress.IsActive = false;
        }

        private async void Logoff_Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.TokenManager.RemoveToken();

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                Windows.Storage.StorageFile dataFile = await localFolder.GetFileAsync("dataFile.txt");
                await dataFile.DeleteAsync();
            }
            catch { }

            this.Frame.Navigate(typeof(LoginPage));
        }

        private void AnnoucementTapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Announcement selectedAnnouncement = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Announcement;

            int moduleIndex = Utils.DataManager.GetModuleIndexByModuleId(selectedAnnouncement.announceModuleId);
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

        private void onNextDaySelected(object sender, TappedRoutedEventArgs e)
        {
            if (dayListIndex < 5)
            {
                prevDay_btn.IsEnabled = true;

                if (dayListIndex == 4)  
                {
                    nextDay_btn.IsEnabled = false;
                }

                dayListIndex++;

                date_textblock.Text = dayList[dayListIndex];

                dailyListView.Source = Utils.DataManager.GetDailyClassList((todayCode + dayListIndex + 4) % 6);
            }
        }

        private void onPrevDaySelected(object sender, TappedRoutedEventArgs e)
        {
            if (dayListIndex > 0)
            {
                nextDay_btn.IsEnabled = true;

                if (dayListIndex == 1)
                {
                    prevDay_btn.IsEnabled = false;
                }

                dayListIndex--;

                date_textblock.Text = dayList[dayListIndex];

                dailyListView.Source = Utils.DataManager.GetDailyClassList((todayCode + dayListIndex + 4) % 6);
            }
        }

        private void timetableItemTapped(object sender, TappedRoutedEventArgs e)
        {
            DataStructure.Class selectedClass = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Class;
            if(selectedClass!=null)
                this.Frame.Navigate(typeof(TimetablePage));
        }

        private void ModuleItemGridEntered(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Module selectedModule = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Module;
            
            if (selectedModule != null)
            {
                if(isRightClicking == false)
                    selectedModule.moduleShowColor = selectedModule.moduleSecondaryColor;
                else
                    selectedModule.moduleShowColor = Color.FromArgb(255, 211, 211, 211);
            }
        }


        private void ModuleItemGridPressed(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Module selectedModule = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Module;
            isRightClicking = true;

            foreach (DataStructure.Module module in Utils.DataManager.modules)
            {
                module.moduleShowColor = module.modulePrimaryColor;
            }

            if (selectedModule != null)
            {
                selectedModule.moduleShowColor = Color.FromArgb(255, 100, 100, 100);
            }
        }

        private void ModuleItemGridExited(object sender, PointerRoutedEventArgs e)
        {
            foreach (DataStructure.Module module in Utils.DataManager.modules)
            {
                module.moduleShowColor = module.modulePrimaryColor;
            }

            isRightClicking = false;
        }

        private void ModuleItemGridReleased(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Module selectedModule = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Module;
            
            if (selectedModule != null)
            {
                selectedModule.moduleShowColor = selectedModule.modulePrimaryColor;
                if (isRightClicking)
                {
                    int moduleIndex = Utils.DataManager.GetModuleIndex(selectedModule);
                    DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, -1);
                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(ItemPage), navParams);
                    }
                }
            }
            isRightClicking = false;
        }

        private void AnnoucementEntered(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Announcement selectedAnnouncement = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Announcement;
            if (selectedAnnouncement != null)
            {
                if (isRightClicking == false)
                {
                    selectedAnnouncement.announceColor = selectedAnnouncement.announceSecondaryColor;
                    selectedAnnouncement.backgroundConverter = 2;
                }
                else
                {
                    selectedAnnouncement.announceColor = selectedAnnouncement.announceSecondaryColor;
                    selectedAnnouncement.backgroundConverter = 3;
                }
            }
        }
        private void AnnoucementExited(object sender, PointerRoutedEventArgs e)
        {
            foreach (DataStructure.Announcement announcement in Utils.DataManager.announcements)
            {
                announcement.announceColor = announcement.announcePrimaryColor;
                announcement.backgroundConverter = 1;
            }
            isRightClicking = false;
        }

        private void AnnoucementPressed(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Announcement selectedAnnouncement = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Announcement;
            
            foreach (DataStructure.Announcement announcement in Utils.DataManager.announcements)
            {
                announcement.announceColor = announcement.announcePrimaryColor;
                announcement.backgroundConverter = 1;
            }

            isRightClicking = true;
            if (selectedAnnouncement != null)
            {
                selectedAnnouncement.announceColor = selectedAnnouncement.announceSecondaryColor;
                selectedAnnouncement.backgroundConverter = 3;
            }
        }

        private void AnnoucementReleased(object sender, PointerRoutedEventArgs e)
        {
            DataStructure.Announcement selectedAnnouncement = (e.OriginalSource as FrameworkElement).DataContext as DataStructure.Announcement;
            
            foreach (DataStructure.Announcement announcement in Utils.DataManager.announcements)
            {
                announcement.announceColor = announcement.announcePrimaryColor;
                announcement.backgroundConverter = 1;
            }
            
            int moduleIndex = Utils.DataManager.GetModuleIndexByModuleId(selectedAnnouncement.announceModuleId);
            int announcementIndex = Utils.DataManager.GetAnnouncementIndex(moduleIndex, selectedAnnouncement.announceID);
            if (isRightClicking)
            {
                DataStructure.NavParams navParams = new DataStructure.NavParams(moduleIndex, announcementIndex);
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(ItemPage), navParams);
                }
            }
            isRightClicking = false;
        }

        private void AnnouncementLayoutUpdated(object sender, object e)
        {
            foreach (DataStructure.Announcement announcement in Utils.DataManager.announcements)
            {
                announcement.announceColor = announcement.announcePrimaryColor;
                announcement.backgroundConverter = 1;
            }
        }

        private void moduleItemLayoutUpdated(object sender, object e)
        {
            foreach(DataStructure.Module module in Utils.DataManager.modules)
            {
                module.moduleShowColor = module.modulePrimaryColor;
            }
        }

    }

    public class AnnouncementBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int _value = (int)value;
            if (_value == 1)
                return "#808080";
            else if (_value == 2)
                return "#696969";
            else
                return "#D3D3D3";
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

