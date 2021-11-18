using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace StudentManagement.Utils
{
    public class FileExtensionToImageConverter : MarkupExtension, IValueConverter
    {
        private static FileExtensionToImageConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value as string;
            string imagePath = "pack://application:,,,/Resources/Images/";
            string fileExt = Path.GetExtension(fileName);
            Uri uri;
            switch (fileExt)
            {
                case ".txt":
                    uri = new Uri(imagePath + "txt.png");
                    break;
                case ".rar":
                case ".zip":
                    uri = new Uri(imagePath + "zip.png");
                    break;
                case ".jpg":
                    uri = new Uri(imagePath + "jpg.png");
                    break;
                case ".png":
                    uri = new Uri(imagePath + "png.png");
                    break;
                case ".pdf":
                    uri = new Uri(imagePath + "pdf.png");
                    break;
                case ".xls":
                case ".xlsx":
                    uri = new Uri(imagePath + "xls.png");
                    break;
                case ".doc":
                case ".docx":
                    uri = new Uri(imagePath + "doc.png");
                    break;
                case ".ppt":
                case ".pptx":
                    uri = new Uri(imagePath + "ppt.png");
                    break;
                default:
                    uri = new Uri(imagePath + "file.png");
                    break;
            }
            return uri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new FileExtensionToImageConverter());
        }
    }
}
