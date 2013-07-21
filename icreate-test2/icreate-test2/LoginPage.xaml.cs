using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.System;
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
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;

using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237
namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : icreate_test2.Common.LayoutAwarePage
    {
        private String username;
        private String password;
        private String domain;
        private String postString;
        private bool isModuleDataUpdated = false;

        public LoginPage()
        {
            this.InitializeComponent();
            hideUserInfo.Begin();

            Utils.DataManager.InitializeDataLists();
        }

        void myStoryboard_Completed(object sender, object e)
        {
            userInfoOpacity.Begin();
            showUserInfo.Begin();
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
            // load userId, password and domain from application setting data
            this.LoadUserCredentials();

            // make sure network connection is available
            if (!IsInternet())
            {
                MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                noInternetDialog.ShowAsync();
            }
            else
            {
                // if there have been token stored
                // load the token
                if (Utils.TokenManager.IsTokenExisting())
                {
                    // disable controls
                    DisableLoginControls();

                    // display progress circle
                    ProgressRing.IsActive = false;
                    ProgressRing_snapped.IsActive = false;

                    // check if token is valid
                    if (await Utils.TokenManager.IsTokenValid())
                    {
                        //if so, active the progressring under splashscreen
                        ProgressRingIfTokenExist.IsActive = true;

                        // load data on modules and classes
                        await GetModulesAsync();
                        await GetTimetableAsync();
                        Utils.DataManager.GenerateDailyClassList();
                        // if so, hide progress circle
                        ProgressRing.IsActive = false;
                        ProgressRing_snapped.IsActive = false;
                        // navigate to main menu page
                        this.Frame.Navigate(typeof(MainPage), isModuleDataUpdated);
                    }
                    else
                    {
                        // if not, hide progress circle
                        // enable controls

                        myStoryboard.Begin();
                        myStoryboard.Completed += myStoryboard_Completed;

                        ProgressRing.IsActive = false;
                        ProgressRing_snapped.IsActive = false;
                        EnableLoginControls();
                    }
                }
                else
                {
                    myStoryboard.Begin();
                    myStoryboard.Completed += myStoryboard_Completed;
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
            base.SaveState(pageState);
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }


        private async void Login()
        {
            if (!IsInternet())
            {
                MessageDialog noInternetDialog = new MessageDialog("There is currently no internet connection..", "Oops");
                await noInternetDialog.ShowAsync();
            }
            else
            {
                // disable controls
                DisableLoginControls();

                // display progress ring
                ProgressRing.IsActive = false;
                ProgressRing_snapped.IsActive = false;

                username = UsernameTextBox.Text;
                password = PasswordBox.Password;
                domain = DomainComboBox.SelectedValue.ToString();

                postString = Utils.LAPI.GeneratePostString(username, password, domain);

                if (await Utils.TokenManager.LoginAsync(postString))
                {
                    //hide everything
                    HideLoginControls();
                    // hide progress ring
                    ProgressRing.IsActive = true;
                    ProgressRing_snapped.IsActive = true;

                    // update token
                    Utils.TokenManager.StoreToken();
                    SaveUserCredentials();
                    
                    // load data on modules and classes
                    await GetModulesAsync();
                    await GetTimetableAsync();

                    // navigate to the home page
                    this.Frame.Navigate(typeof(MainPage), isModuleDataUpdated);
                }
                else
                {
                    // hide progress ring
                    // enable log in controls
                    ProgressRing.IsActive = false;
                    ProgressRing_snapped.IsActive = false;
                    EnableLoginControls();

                    MessageDialog loginFailDialog = new MessageDialog("Log in failed. Please check your userId and password", "Oops");
                    await loginFailDialog.ShowAsync();
                }
            }
        }

        // loading user credential from application data settings
        private void LoadUserCredentials()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values.ContainsKey("userID"))
            {
                UsernameTextBox.Text = roamingSettings.Values["userID"].ToString();
                UsernameTextBox_snapped.Text = roamingSettings.Values["userID"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("domain"))
            {
                DomainComboBox.SelectedItem = roamingSettings.Values["domain"].ToString();
                DomainComboBox_snapped.SelectedItem = roamingSettings.Values["domain"].ToString();
            }
        }

        // saving user credentials to application data settings
        private void SaveUserCredentials()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            roamingSettings.Values["userID"] = this.username;
            roamingSettings.Values["domain"] = this.domain;
        }

        // disable log in page controls
        private void DisableLoginControls()
        {
            UsernameTextBox.IsEnabled = false;
            UsernameTextBox_snapped.IsEnabled = false;
            PasswordBox.IsEnabled = false;
            PasswordBox_snapped.IsEnabled = false;
            DomainComboBox.IsEnabled = false;
            DomainComboBox_snapped.IsEnabled = false;
            LoginButton.IsEnabled = false;
            LoginButton_snapped.IsEnabled = false;
        }

        // enable log in page controls
        private void EnableLoginControls()
        {
            UsernameTextBox.IsEnabled = true;
            UsernameTextBox_snapped.IsEnabled = true;
            PasswordBox.IsEnabled = true;
            PasswordBox_snapped.IsEnabled = true;
            DomainComboBox.IsEnabled = true;
            DomainComboBox_snapped.IsEnabled = true;
            LoginButton.IsEnabled = true;
            LoginButton_snapped.IsEnabled = true;
        }
        private void HideLoginControls()
        {
            userInfo.Opacity = 0;
            userInfo_snapped.Opacity = 0;
        }

        private async Task GetClassesAsync()
        {
            foreach (DataStructure.SemesterInfo sem in Utils.DataManager.GetSems())
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("AcadYear", sem.AcademicYear);
                parameters.Add("Semester", sem.Semester);

                string classesResponse = await Utils.RequestSender.GetResponseStringAsync("Timetable_Student", parameters);

                if (classesResponse != null)
                {
                    DataStructure.ClassWrapper classWrapper = JsonConvert.DeserializeObject<DataStructure.ClassWrapper>(classesResponse);

                    if (classWrapper.comments.Equals("Valid login!"))
                    {
                        foreach (DataStructure.Class mClass in classWrapper.classes)
                        {
                            Utils.DataManager.AddClass(mClass);
                        }
                    }
                }
            }
        }

        private async Task GetTimetableAsync()
        {
            List<int> ids = new List<int>();
            int count = Utils.DataManager.GetModules().Count;

            for(int i=0; i<count; i++)
            {
                ids.Add(i);
            }
            await Task.WhenAll(ids.Select(i => GetTimetableForOneModuleAsync(Utils.DataManager.GetModuleAt(i))));
        }

        private async Task GetTimetableForOneModuleAsync(DataStructure.Module module)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CourseId", module.moduleId);

            string timetableResponse = await Utils.RequestSender.GetResponseStringAsync("Timetable_Student_Module", parameters);

            if (timetableResponse != null)
            {
                DataStructure.ClassWrapper classWrapper = JsonConvert.DeserializeObject<DataStructure.ClassWrapper>(timetableResponse);

                if (classWrapper.comments.Equals("Valid login!"))
                {
                    foreach (DataStructure.Class mClass in classWrapper.classes)
                    {
                        mClass.classModuleColor = module.modulePrimaryColor;
                        Utils.DataManager.AddClass(mClass);
                    }
                }
            }
        }


        private async Task GetModulesAsync()
        {
            int iterator = 0;
            string modulesResponse;

            // try to fetch old data from local storage
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                modulesResponse = await FileIO.ReadTextAsync(sampleFile);
                // Data is contained in timestamp
            }
            catch (Exception)
            {
                modulesResponse = null;
            }

            // if serialized data does not exist
            // fetch module data from server
            if (modulesResponse == null)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Duration", "0");
                parameters.Add("IncludeAllInfo", "true");

                modulesResponse = await Utils.RequestSender.GetResponseStringAsync("Modules", parameters);

                StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, modulesResponse);

                isModuleDataUpdated = true;
            }

            // deserialize data into objects
            DataStructure.ModuleInfoWrapper moduleWrapper = JsonConvert.DeserializeObject<DataStructure.ModuleInfoWrapper>(modulesResponse);

            if (moduleWrapper.comments.Equals("Valid login!"))
            {
                foreach (DataStructure.Module module in moduleWrapper.modules)
                {
                    foreach (DataStructure.Announcement announcement in module.moduleAnnouncements)
                    {
                        announcement.GenerateDisplayContent(module);
                        Utils.DataManager.AddAnnouncement(announcement);
                    }
                    module.SetModuleColor(DataStructure.Colors.GetModuleColor(iterator), DataStructure.Colors.GetSecondaryColor(iterator));

                    DataStructure.SemesterInfo newSemInfo = new DataStructure.SemesterInfo(module.moduleAcadYear,
                                                                                           module.moduleSemester.Replace("Semester ", String.Empty));

                    Utils.DataManager.AddSemInfo(newSemInfo);
                    Utils.DataManager.AddModule(module);
                    iterator++;
                }
            }
        }

        private void onPasswordKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Login();
            }
        }

        // to check internet availability
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }
}
