using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace StudentManagement.Components
{
    /// <summary>
    /// Interaction logic for CreatePostNewfeed.xaml
    /// </summary>
    public partial class CreatePostNewfeed : UserControl
    {
        public CreatePostNewfeed()
        {
            InitializeComponent();
        }
    }

    //public class RichTextBoxHelper : DependencyObject
    //{
    //    public static string GetText(DependencyObject obj)
    //    {
    //        return (string)obj.GetValue(TextProperty);
    //    }

    //    public static void SetText(DependencyObject obj, string value)
    //    {
    //        obj.SetValue(TextProperty, value);
    //    }

    //    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    //    public static readonly DependencyProperty TextProperty =
    //        DependencyProperty.Register("Text", typeof(string), typeof(RichTextBoxHelper),
    //            new FrameworkPropertyMetadata( // Property metadata
    //                            "Hi", // default value
    //                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
    //                            new PropertyChangedCallback(OnTextPropertyChanged)    // property changed callback
    //                            ));

    //    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {

    //        var richTextBox = (RichTextBox)d;
    //        var doc = new FlowDocument();
    //        doc.Blocks.Add(new Paragraph(new Run("Tes thu")));
    //        richTextBox.Document = doc;
    //        //// Parse the XAML to a document (or use XamlReader.Parse())
    //        //var xaml = GetText(richTextBox);
    //        //Console.WriteLine(xaml);
    //        //var doc = new FlowDocument();
    //        //var range = new TextRange(doc.ContentStart, doc.ContentEnd);

    //        //range.Load(new MemoryStream(Encoding.UTF8.GetBytes(xaml)),
    //        //      DataFormats.Xaml);

    //        //// Set the document
    //        //richTextBox.Document = doc;

    //        //// When the document changes update the source
    //        //range.Changed += (obj2, e2) =>
    //        //{
    //        //    if (richTextBox.Document == doc)
    //        //    {
    //        //        MemoryStream buffer = new MemoryStream();
    //        //        range.Save(buffer, DataFormats.Xaml);
    //        //        SetText(richTextBox,
    //        //            Encoding.UTF8.GetString(buffer.ToArray()));
    //        //    }
    //        //};
    //    }
    //}
}
