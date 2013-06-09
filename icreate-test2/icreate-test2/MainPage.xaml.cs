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
            hideUserInfo.Begin();
            myStoryboard.Begin();
            myStoryboard.Completed += myStoryboard_Completed;
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
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("userID"))
            {
                userID.Text = roamingSettings.Values["userID"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("password"))
            {
                password.Password = roamingSettings.Values["password"].ToString();
            }
            if (roamingSettings.Values.ContainsKey("domain"))
            {
                domain.SelectedItem = roamingSettings.Values["domain"].ToString();
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

        private void login_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
        }
        private void userIDChanged(object sender, TextChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["userID"] = userID.Text;
            
        }

        private void passwordChanged(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["password"] = password.Password;
        }

        private void domainSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["domain"] = domain.SelectedItem;
        }
    }
}
