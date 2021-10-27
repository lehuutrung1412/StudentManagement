using StudentManagement.Views;
using System.Windows;

namespace StudentManagement
{
    /// <summary>
    /// Displays a message box.
    /// </summary>
    public sealed class MyMessageBox
    {
        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon;
        /// and that accepts a default message box result, complies with the specified options,
        /// and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttonsto display.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MyMessageBoxView msgWindow = new MyMessageBoxView(messageBoxText, caption, button, icon);
            _ = msgWindow.ShowDialog();

            return msgWindow.Result;
        }
        /// <summary>
        /// Displays a message box that has a message, title bar caption and button;
        /// and that accepts a default message box result, complies with the specified options,
        /// and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttonsto display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            MyMessageBoxView msgWindow = new MyMessageBoxView(messageBoxText, caption, button);
            _ = msgWindow.ShowDialog();

            return msgWindow.Result;
        }
        /// <summary>
        /// Displays a message box that has a message and title bar caption;
        /// and that accepts a default message box result, complies with the specified options,
        /// and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            MyMessageBoxView msgWindow = new MyMessageBoxView(messageBoxText, caption);
            _ = msgWindow.ShowDialog();

            return msgWindow.Result;
        }
        /// <summary>
        /// Displays a message box that has a message;
        /// and that accepts a default message box result, complies with the specified options,
        /// and returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText)
        {
            MyMessageBoxView msgWindow = new MyMessageBoxView(messageBoxText);
            _ = msgWindow.ShowDialog();

            return msgWindow.Result;
        }
    }
}
