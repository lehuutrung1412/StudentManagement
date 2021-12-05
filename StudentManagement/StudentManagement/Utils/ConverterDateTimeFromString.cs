using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ConverterDateTimeFromString : MarkupExtension, IValueConverter
    {
        private static ConverterDateTimeFromString _instance;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;
            DateTime tmp;
            if (DateTime.TryParse(value.ToString(), out tmp))
                return tmp;
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DateTime tmp;
            if(!DateTime.TryParse(value.ToString(), out tmp))
                return "";
            return ((DateTime)value).ToString("dd / MM / yyyy");
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new ConverterDateTimeFromString());
        }
    }
}
