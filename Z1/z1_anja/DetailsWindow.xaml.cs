using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace z1_anja
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {

        public DetailsWindow(int index)
        {
            int id;
            id = MainWindow.Rings[index].ID;
            string path = "rtb" + id + ".rtf";
            TextRange range;
            FileStream fStream;

            InitializeComponent();

            range = new TextRange(RichTextBoxDetails.Document.ContentStart, RichTextBoxDetails.Document.ContentEnd);
            fStream = new FileStream(path, FileMode.OpenOrCreate);
            range.Load(fStream, DataFormats.Rtf);
            fStream.Close();
            Name.Content = MainWindow.Rings[index].Name;
            Image.Source = new BitmapImage(new Uri(MainWindow.Rings[index].PathImage));
        }

        private void Button_ClickX(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
