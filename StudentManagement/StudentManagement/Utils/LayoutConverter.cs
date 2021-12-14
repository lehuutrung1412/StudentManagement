using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class LayoutConverter : MarkupExtension, IValueConverter
    {
        private static LayoutConverter _instance;

        public List<Dictionary<string, string>> ListLayout = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>()
            {
                {"left", "2*"},
                {"center", "7*"},
                {"right", "3*"},
            },
            new Dictionary<string, string>()
            {
                {"left", "2*"},
                {"center", "10*"},
                {"right", "0*"},
            }
        };

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LayoutViewModel layoutViewModel = MainViewModel.Instance.LayoutViewModel;

            int layoutType = 0;
            if (layoutViewModel.ContentViewModel is SettingUserInfoViewModel)
            {
                layoutType = 1;
            }

            if (layoutViewModel.ContentViewModel is UserInfoViewModel)
            {
                layoutType = 1;
            }

            return ListLayout[layoutType][parameter.ToString()];
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new LayoutConverter());
        }
    }
}
