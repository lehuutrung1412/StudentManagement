using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace StudentManagement.Views
{
    /// <summary>
    /// Interaction logic for MyMessageBoxView.xaml
    /// </summary>
    public partial class MyMessageBoxView : Window
    {
        public MyMessageBoxView(string messageBoxText)
        {
            InitializeComponent();

            txblContent.Text = messageBoxText;
            DisplayButtons(MessageBoxButton.OK);
        }

        public MyMessageBoxView(string messageBoxText, string caption)
        {
            InitializeComponent();

            txblContent.Text = messageBoxText;
            (TitleBar.FindName("txblTitle") as TextBlock).Text = caption;
            DisplayButtons(MessageBoxButton.OK);
        }

        public MyMessageBoxView(string messageBoxText, string caption, MessageBoxButton button)
        {
            InitializeComponent();

            txblContent.Text = messageBoxText;
            (TitleBar.FindName("txblTitle") as TextBlock).Text = caption;
            DisplayButtons(button);
        }

        public MyMessageBoxView(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            InitializeComponent();

            txblContent.Text = messageBoxText;
            (TitleBar.FindName("txblTitle") as TextBlock).Text = caption;
            DisplayButtons(button);
            DisplayIcon(icon);
        }

        public MessageBoxResult Result { get; set; }

        #region Methods

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all except OK, Cancel
                    btnOk.Visibility = Visibility.Visible;
                    btnOk.Focus();
                    btnCancel.Visibility = Visibility.Visible;

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    // Hide all except Yes, No
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    btnNo.Visibility = Visibility.Visible;

                    btnOk.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNoCancel:
                    // Hide only OK
                    btnYes.Visibility = Visibility.Visible;
                    btnYes.Focus();
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;

                    btnOk.Visibility = Visibility.Collapsed;
                    break;
                default:
                    // Hide all except OK
                    btnOk.Visibility = Visibility.Visible;
                    btnOk.Focus();

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void DisplayIcon(MessageBoxImage image)
        {
            BitmapImage icon;

            switch (image)
            {
                case MessageBoxImage.Warning:
                    icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/warning-sign.png"));
                    break;
                case MessageBoxImage.Error:
                    icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/cancel.png"));
                    break;
                case MessageBoxImage.Question:
                    icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/question-mark.png"));
                    break;
                default:
                    icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/information.png"));
                    break;
            }

            msgIcon.Source = icon;
            msgIcon.Visibility = Visibility.Visible;
        }

        #endregion Methods

        #region Events
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }
        #endregion Events
    }
}
