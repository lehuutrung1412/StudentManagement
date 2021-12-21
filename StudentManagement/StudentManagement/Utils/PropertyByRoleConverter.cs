using StudentManagement.Services;
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
    public class PropertyByRoleConverter : MarkupExtension, IValueConverter
    {
        private static PropertyByRoleConverter _instance;

        public Dictionary<string, List<object>> PropertyByRoleList = new Dictionary<string, List<object>>() {
            {"Visibility", new List<object> { Visibility.Visible, Visibility.Visible, Visibility.Collapsed }},
            {"Visibility2", new List<object> { Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed }},
            {"IsEnabled", new List<object> { true, false, false }},
            {"SearchBarOneButton", new List<object> { "1.5*", "0", "0"}},
            {"FileManagerRSB", new List<object> { "*", "*", "Auto"}},
            {"Lesson", new List<object> { "", "tiết dạy", "tiết học"}},
            };

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int role;

            switch (LoginServices.CurrentUser.UserRole.Role)
            {
                case "Admin":
                    role = 0;
                    break;
                case "Giáo viên":
                    role = 1;
                    break;
                default:
                    role = 2;
                    break;
            }

            return PropertyByRoleList[parameter.ToString()][role];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new PropertyByRoleConverter());
        }
    }
}
