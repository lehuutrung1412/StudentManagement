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
    public class FileSizeConverter : MarkupExtension, IValueConverter
    {
        private static FileSizeConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long fileSize = (long)value;
            string sizeToString;
            if ((float)fileSize / 1048576 > 1)
            {
                sizeToString = ((int)((float)fileSize / 1048576)).ToString() + " MB";
            }
            else if ((float)fileSize / 1024 > 1)
            {
                sizeToString = ((int)((float)fileSize / 1024)).ToString() + " KB";
            }
            else if (fileSize > 0)
            {
                sizeToString = "1 KB";
            }
            else
            {
                sizeToString = "0 KB";
            }
            return sizeToString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FileSizeConverter());
        }
    }
}
