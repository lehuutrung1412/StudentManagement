using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class DateTimeConverter : MarkupExtension, IValueConverter
    {
        private readonly CultureInfo _culture = new CultureInfo("vi-VN");
        private static DateTimeConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as string)
            {
                case "full":
                    return GetFullTime(value);
                default:
                    return GetRelativeTime(value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new DateTimeConverter());
        }

        private string GetRelativeTime(object value)
        {
            DateTime dateTime = (DateTime)value;
            DateTime now = DateTime.Parse(DateTime.Now.ToString(), _culture);
            TimeSpan dateTimeValue = now.Subtract(dateTime);
            string result = dateTime.ToString("dd") + " tháng " + dateTime.ToString("MM") + ", " + dateTime.ToString("yyyy");
            if (dateTimeValue.Days > 0)
            {
                if (dateTimeValue.Days <= 3)
                {
                    result = dateTimeValue.Days.ToString() + " ngày trước";
                }
            }
            else
            {
                result = dateTimeValue.Hours > 0
                    ? dateTimeValue.Hours.ToString() + " giờ trước"
                    : dateTimeValue.Minutes > 0 ? dateTimeValue.Minutes.ToString() + " phút trước" : "Bây giờ";
            }
            return result;
        }

        private string GetFullTime(object value)
        {
            DateTime dateTime = (DateTime)value;
            return ConvertDayOfWeek(dateTime.DayOfWeek) + ", " + dateTime.ToString("dd") + " tháng " + dateTime.ToString("MM") + ", " + dateTime.ToString("yyyy") + " vào lúc " + dateTime.ToString("HH") + ":" + dateTime.ToString("mm") + ":" + dateTime.ToString("ss");
        }

        private string ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch ((int)dayOfWeek)
            {
                case 1:
                    return "Thứ hai";
                case 2:
                    return "Thứ ba";
                case 3:
                    return "Thứ tư";
                case 4:
                    return "Thứ năm";
                case 5:
                    return "Thứ sáu";
                case 6:
                    return "Thứ bảy";
                default:
                    return "Chủ nhật";
            }
        }
    }
}
