using StudentManagement.Services;
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
    public class IntToDateOfWeek : MarkupExtension, IValueConverter
    {
        private static IntToDateOfWeek _instance;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SubjectClassServices.Instance.DayOfWeeks[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new IntToDateOfWeek());
        }
    }
}
