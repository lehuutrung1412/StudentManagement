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
    public class VisibilityByUserIdConverter : MarkupExtension, IValueConverter
    {
        private static VisibilityByUserIdConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userId = (Guid)value;
            return UserServices.Instance.GetUserById(userId).UserRole.Role;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new VisibilityByUserIdConverter());
        }
    }
}
