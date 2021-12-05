using StudentManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class CheckAdminConverter : MarkupExtension, IValueConverter
    {
        private static CheckAdminConverter _instance;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return UserServices.Instance.CheckAdminByIdUser((Guid)value);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new CheckAdminConverter());
        }
    }
}
