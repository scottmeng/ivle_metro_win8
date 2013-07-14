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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace icreate_test2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TimetablePage : icreate_test2.Common.LayoutAwarePage
    {
        public TimetablePage()
        {
            this.InitializeComponent();
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
            foreach (DataStructure.Class mClass in Utils.DataManager.GetClasses())
            {
                // styling of timetable item
                Border classBorder = new Border();
                classBorder.Background = new SolidColorBrush(mClass.classModuleColor);

                TextBlock classBlock = new TextBlock();
                classBlock.Text = mClass.classModuleCode + "    " + mClass.classLessonType;
                classBlock.FontSize = 24;
                classBlock.FontFamily = new FontFamily("Segoe UI");
                classBlock.Foreground = new SolidColorBrush(Colors.White);

                classBorder.Child = classBlock;

                // set timetable item column position
                Grid.SetColumn(classBorder, mClass.classDayCodeInt);

                // set timetable item row position
                int startTimeInHours = mClass.classStartTimeInt / 100 * 100 + (mClass.classStartTimeInt % 100) * 100 / 60;
                Grid.SetRow(classBorder, (startTimeInHours - 800) / 50 + 1);

                // set timetable item row span
                int durationInHours = mClass.classDurationInt / 100 * 100 + (mClass.classDurationInt % 100) * 100 / 60;
                Grid.SetRowSpan(classBorder, durationInHours / 50);

                timetableGrid.Children.Add(classBorder);
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
    }
}
