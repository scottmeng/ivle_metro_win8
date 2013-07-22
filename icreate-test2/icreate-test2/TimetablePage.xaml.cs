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
            timetableGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            int todayCode = DateTime.Now.DayOfWeek.GetHashCode();
            SolidColorBrush bluegreen = new SolidColorBrush(Color.FromArgb(255, 71,165,168));

            switch (todayCode)
            {
                case 1:
                    mon.Foreground = bluegreen;
                    mon.FontSize = 22;
                    mon.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                case 2:
                    tue.Foreground = bluegreen;
                    tue.FontSize = 22;
                    tue.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                case 3:
                    wed.Foreground = bluegreen;
                    wed.FontSize = 22;
                    wed.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                case 4:
                    thu.Foreground = bluegreen;
                    thu.FontSize = 22;
                    thu.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                case 5:
                    fri.Foreground = bluegreen;
                    fri.FontSize = 22;
                    fri.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                case 6:
                    sat.Foreground = bluegreen;
                    sat.FontSize = 22;
                    sat.FontWeight = Windows.UI.Text.FontWeights.Bold;
                    break;
                default:
                    break;
            }

            for (int i = 1; i < 7; i++)
            {
                SolidColorBrush backgroundBrush;

                if (i == todayCode)
                {
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(255, 234, 234, 234));
                }
                else
                {
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }

                for (int j = 1; j < 29; j += 2)
                {
                    Border myBorder = new Border();

                    myBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    myBorder.BorderThickness = new Thickness(0.5);
                    myBorder.Background = backgroundBrush;
                    timetableGrid.Children.Add(myBorder);
                    Grid.SetColumn(myBorder, i);
                    Grid.SetRow(myBorder, j);
                    Grid.SetRowSpan(myBorder, 2);
                }
            }
            for (int i = 0; i < 7; i++)
            {
                Border myBorder = new Border();

                myBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
                myBorder.BorderThickness = new Thickness(0.5);
                timetableGrid.Children.Add(myBorder);
                Grid.SetColumn(myBorder, i);
                Grid.SetRow(myBorder, 0);
            }
            for (int i = 0; i < 29; i++)
            {
                Border myBorder = new Border();
                SolidColorBrush backgroundBrush;
                backgroundBrush = new SolidColorBrush(Color.FromArgb(255, 234, 234, 234));
                myBorder.BorderBrush = new SolidColorBrush(Colors.LightGray);
                myBorder.BorderThickness = new Thickness(0.5);
                timetableGrid.Children.Add(myBorder);
                Grid.SetColumn(myBorder, 0);
                Grid.SetRow(myBorder, i);
            }


            foreach (DataStructure.Class mClass in Utils.DataManager.GetClasses())
            {
                // styling of timetable item
                Border classBorder = new Border();
                classBorder.Margin = new Thickness(2);
                classBorder.Background = new SolidColorBrush(mClass.classModuleColor);
                CornerRadius radius = new CornerRadius(20);
                classBorder.CornerRadius = radius;

                StackPanel oneClass = new StackPanel();
                oneClass.Orientation = Orientation.Vertical;
                classBorder.Child = oneClass;

                TextBlock classBlock = new TextBlock();
                classBlock.Padding = new Thickness(2);
                classBlock.Margin = new Thickness(5, 0, 0, 0);
                classBlock.Text = mClass.classModuleCode + "    " + mClass.classLessonType;
                classBlock.FontSize = 22;
                classBlock.FontFamily = new FontFamily("Segoe UI");
                classBlock.Foreground = new SolidColorBrush(Colors.White);

                oneClass.Children.Add(classBlock);

                TextBlock location = new TextBlock();
                location.Text = mClass.classVenue;
                location.FontSize = 18;
                location.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                location.Foreground = new SolidColorBrush(Colors.White);
                oneClass.Children.Add(location);

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

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            GoBack(sender, e);
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
    }
}
