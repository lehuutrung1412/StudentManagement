using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class BooleanToVisibilityConverter :MarkupExtension, IValueConverter
    {
        private static BooleanToVisibilityConverter _instance;
        private object GetVisibility(object value)
        {
            if (!(value is bool))
                return Visibility.Hidden;
            bool objValue = (bool)value;
            if (objValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new BooleanToVisibilityConverter());
        }
    }
}
