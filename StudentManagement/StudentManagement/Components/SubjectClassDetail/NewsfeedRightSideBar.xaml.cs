using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            SetIsAbsentDay(d, CheckAbsentDay((DateTime)d.GetValue(DateProperty), d));
            SetIsMakeUpDay(d, CheckMakeUpDay((DateTime)d.GetValue(DateProperty), d));
            SetIsScheduleDay(d, CheckScheduleDay((DateTime)d.GetValue(DateProperty), d));
        }

        private static bool CheckAbsentDay(DateTime date, DependencyObject d)
        {
            try
            {
                ObservableCollection<DateTime> dateTimes = (ObservableCollection<DateTime>)d.GetValue(ListAbsentProperty);
                return dateTimes.Any(dateItem => dateItem == date);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CheckMakeUpDay(DateTime date, DependencyObject d)
        {
            try
            {
                ObservableCollection<DateTime> dateTimes = (ObservableCollection<DateTime>)d.GetValue(ListMakeUpProperty);
                return dateTimes.Any(dateItem => dateItem == date);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool CheckScheduleDay(DateTime date, DependencyObject d)
        {
            try
            {
                ObservableCollection<DateTime> dateTimes = (ObservableCollection<DateTime>)d.GetValue(ListScheduleProperty);
                return dateTimes.Any(dateItem => dateItem == date);
            }
            catch (Exception)
            {
                return false;
            }
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

        public static DependencyProperty ListScheduleProperty = DependencyProperty.RegisterAttached(
            "ListSchedule", typeof(ObservableCollection<DateTime>), typeof(CalendarHelper));

        public static ObservableCollection<DateTime> GetListSchedule(DependencyObject obj)
        {
            return (ObservableCollection<DateTime>)obj.GetValue(ListScheduleProperty);
        }

        public static void SetListSchedule(DependencyObject obj, ObservableCollection<DateTime> value)
        {
            obj.SetValue(ListScheduleProperty, value);
        }

        public static DependencyProperty ListAbsentProperty = DependencyProperty.RegisterAttached(
            "ListAbsent", typeof(ObservableCollection<DateTime>), typeof(CalendarHelper));

        public static ObservableCollection<DateTime> GetListAbsent(DependencyObject obj)
        {
            return (ObservableCollection<DateTime>)obj.GetValue(ListAbsentProperty);
        }

        public static void SetListAbsent(DependencyObject obj, ObservableCollection<DateTime> value)
        {
            obj.SetValue(ListAbsentProperty, value);
        }

        public static DependencyProperty ListMakeUpProperty = DependencyProperty.RegisterAttached(
            "ListMakeUp", typeof(ObservableCollection<DateTime>), typeof(CalendarHelper));

        public static ObservableCollection<DateTime> GetListMakeUp(DependencyObject obj)
        {
            return (ObservableCollection<DateTime>)obj.GetValue(ListMakeUpProperty);
        }

        public static void SetListMakeUp(DependencyObject obj, ObservableCollection<DateTime> value)
        {
            obj.SetValue(ListMakeUpProperty, value);
        }
    }
}
