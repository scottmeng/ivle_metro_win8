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
    public sealed partial class ItemPage : icreate_test2.Common.LayoutAwarePage
    {
        private List<DataStructure.Module> modules;
        private List<DataStructure.Announcement> recentAnnouncements;
        private List<DataStructure.Class> classes;
        private List<DataStructure.SemesterInfo> sems;

        //temp
        private List<object> week;

        public ItemPage()
        {
            this.InitializeComponent();

            modules = new List<DataStructure.Module>();
            recentAnnouncements = new List<DataStructure.Announcement>();
            classes = new List<DataStructure.Class>();
            sems = new List<DataStructure.SemesterInfo>();

            //temp
            week = new List<object>();
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
            mainModuleName.Text = "CS3233";
            List<moduleName> tempGrid = new List<moduleName>();
            tempGrid.Add(new moduleName() { moduleCode = "EE3204" }); tempGrid.Add(new moduleName() { moduleCode = "EE3204" });
            tempGrid.Add(new moduleName() { moduleCode = "EE3204" }); tempGrid.Add(new moduleName() { moduleCode = "EE3204" });
            moduleNameGridView.Source = tempGrid;
            List<itemNames> tempList = new List<itemNames>();
            tempList.Add(new itemNames() { itemName = "Announcement" }); tempList.Add(new itemNames() { itemName = "Workbin" });
            tempList.Add(new itemNames() { itemName = "MoutiMedia" }); tempList.Add(new itemNames() { itemName = "WebLink" });
            tempList.Add(new itemNames() { itemName = "WebLink" }); tempList.Add(new itemNames() { itemName = "WebLink" });
            tempList.Add(new itemNames() { itemName = "WebLink" }); tempList.Add(new itemNames() { itemName = "WebLink" }); tempList.Add(new itemNames() { itemName = "WebLink" });
            tempList.Add(new itemNames() { itemName = "WebLink" });
            itemListView.Source = tempList;
            if (itemList.SelectedIndex == 1)
            {
                announcementFlipView.SelectedIndex = itemList.SelectedIndex;
            }

            await GetModulesAsync();
            await GetClassesAsync();

            this.recentAnnouncements.Sort(new AnnouncementTimeComparer());


            announcementListView.ItemsSource = recentAnnouncements;

            //temp
            //dailyListView.Source = modules;
        }
        public class itemNames
        {
            public string itemName { get; set; }

        }
        public class moduleName
        {
            public string moduleCode { get; set; }
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
        private async Task GetModulesAsync()
        {
            int iterator = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Duration", "0");
            parameters.Add("IncludeAllInfo", "true");

            String modulesResponse = await Utils.RequestSender.GetResponseStringAsync("Modules", parameters);
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
                    module.SetModuleColor(DataStructure.Colors.GetModuleColor(iterator));

                    DataStructure.SemesterInfo newSemInfo = new DataStructure.SemesterInfo(module.moduleAcadYear,
                                                                                           module.moduleSemester.Replace("Semester ", String.Empty));
                    if (!sems.Contains(newSemInfo, new SeminfoEqualityComparer()))
                    {
                        sems.Add(newSemInfo);
                    }

                    this.modules.Add(module);
                    iterator++;
                }

                //temp
                week.Add(modules);
                week.Add(modules);

            }
        }
        private async Task GetClassesAsync()
        {
            foreach (DataStructure.SemesterInfo sem in sems)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("AcadYear", sem.AcademicYear);
                parameters.Add("Semester", sem.Semester);

                String classesResponse = await Utils.RequestSender.GetResponseStringAsync("Timetable_Student", parameters);
                DataStructure.ClassWrapper classWrapper = JsonConvert.DeserializeObject<DataStructure.ClassWrapper>(classesResponse);

                if (classWrapper.comments.Equals("Valid login!"))
                {
                    foreach (DataStructure.Class mClass in classWrapper.classes)
                    {
                        mClass.GenerateDisplay();
                        classes.Add(mClass);
                    }
                }

            }
        }
        class AnnouncementTimeComparer : IComparer<DataStructure.Announcement>
        {
            // Compares the published date of the announcement
            public int Compare(DataStructure.Announcement announce1, DataStructure.Announcement announce2)
            {
                if (announce1.announceTime.CompareTo(announce2.announceTime) != 0)
                {
                    return announce1.announceTime.CompareTo(announce2.announceTime);
                }
                else if (announce1.announceIsRead.CompareTo(announce2.announceIsRead) != 0)
                {
                    return announce1.announceIsRead.CompareTo(announce2.announceIsRead);
                }
                else
                {
                    return announce1.announceName.CompareTo(announce2.announceName);
                }
            }
        }
        class SeminfoEqualityComparer : IEqualityComparer<DataStructure.SemesterInfo>
        {
            public bool Equals(DataStructure.SemesterInfo s1, DataStructure.SemesterInfo s2)
            {
                if (s1.Semester == s2.Semester && s1.AcademicYear == s2.AcademicYear)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(DataStructure.SemesterInfo sem)
            {
                int hCode = sem.AcademicYear.Length ^ sem.Semester.Length;
                return hCode.GetHashCode();
            }
        }

    }
}
