using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for NewsfeedRightSideBar.xaml
    /// </summary>
    public partial class NewsfeedRightSideBar : UserControl
    {
        public NewsfeedRightSideBar()
        {
            InitializeComponent();
        }
    }

    public static class CalendarHelper
    {
        public static DependencyProperty DateProperty = DependencyProperty.RegisterAttached(
            "Date", typeof(DateTime), typeof(CalendarHelper), new PropertyMetadata { PropertyChangedCallback = DatePropertyChanged });

        public static DateTime GetDate(DependencyObject obj)
        {
            return (DateTime)obj.GetValue(DateProperty);
        }

        public static void SetDate(DependencyObject obj, DateTime value)
        {
            obj.SetValue(DateProperty, value);
        }

        private static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetIsAbsentDay(d, CheckAbsentDay((DateTime)d.GetValue(DateProperty)));
            SetIsMakeUpDay(d, CheckMakeUpDay((DateTime)d.GetValue(DateProperty)));
            SetIsScheduleDay(d, CheckScheduleDay((DateTime)d.GetValue(DateProperty)));
        }

        private static bool CheckAbsentDay(DateTime date)
        {
            List<DateTime> list = new List<DateTime>() { new DateTime(2021, 10, 15), new DateTime(2021, 10, 18) };
            return list.Any(dateItem => dateItem == date);
        }

        private static bool CheckMakeUpDay(DateTime date)
        {
            List<DateTime> list = new List<DateTime>() { new DateTime(2021, 10, 16), new DateTime(2021, 10, 22) };
            return list.Any(dateItem => dateItem == date);
        }

        private static bool CheckScheduleDay(DateTime date)
        {
            List<DateTime> list = new List<DateTime>() { new DateTime(2021, 11, 28), new DateTime(2021, 11, 21) };
            return list.Any(dateItem => dateItem == date);
        }

        public static DependencyProperty IsAbsentProperty = DependencyProperty.RegisterAttached(
            "IsAbsentDay", typeof(bool), typeof(CalendarHelper));

        public static bool GetIsAbsentDay(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAbsentProperty);
        }

        public static void SetIsAbsentDay(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAbsentProperty, value);
        }

        public static DependencyProperty IsMakeUpProperty = DependencyProperty.RegisterAttached(
            "IsMakeUpDay", typeof(bool), typeof(CalendarHelper));

        public static bool GetIsMakeUpDay(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMakeUpProperty);
        }

        public static void SetIsMakeUpDay(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMakeUpProperty, value);
        }

        public static DependencyProperty IsScheduleProperty = DependencyProperty.RegisterAttached(
            "IsScheduleDay", typeof(bool), typeof(CalendarHelper));

        public static bool GetIsScheduleDay(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsScheduleProperty);
        }

        public static void SetIsScheduleDay(DependencyObject obj, bool value)
        {
            obj.SetValue(IsScheduleProperty, value);
        }
    }
}
