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
    public class SwitchBoolConverter : MarkupExtension, IValueConverter
    {
        private static SwitchBoolConverter _instance;

        public SwitchBoolConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool switchBool = !System.Convert.ToBoolean(value);
            return switchBool;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { // read only converter...
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new SwitchBoolConverter());
        }

    }
}
