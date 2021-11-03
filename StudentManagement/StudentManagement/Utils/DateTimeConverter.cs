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
            DateTime dateTime = (DateTime)value;
            DateTime now = DateTime.Parse(DateTime.Now.ToString(), _culture);
            TimeSpan dateTimeValue = now.Subtract(dateTime);
            string result = dateTime.ToLongDateString();
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new DateTimeConverter());
        }
    }
}
