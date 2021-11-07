using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class MultiCommandParameterConverter : MarkupExtension, IMultiValueConverter
    {
        private static MultiCommandParameterConverter _instance;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new MultiCommandParameterConverter());
        }
    }
}