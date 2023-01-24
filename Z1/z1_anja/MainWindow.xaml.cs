using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Classes;

namespace z1_anja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataIO serializer = new DataIO();

        public static BindingList<Ring> Rings { get; set; }

        public MainWindow()
        {
            
            Rings = serializer.DeSerializeObject<BindingList<Ring>>("rings.xml");   
            if(Rings == null)
            {
                Rings = new BindingList<Ring>();
            }
            DataContext = this;

            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {

            serializer.SerializeObject<BindingList<Ring>>(Rings, "rings.xml");
            this.Close();
        }

        private void Button_Click_AddRing(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();

        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Window DetailWindow = new DetailsWindow(DataGrid.SelectedIndex);
            DetailWindow.ShowDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            if (Rings.Count > 0)
            {
                Window EditWIndow = new AddWindow(DataGrid.SelectedIndex);
                EditWIndow.ShowDialog();
                DataGrid.Items.Refresh(); 
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Rings.Count > 0)
            {
                System.IO.File.Delete(Rings[DataGrid.SelectedIndex].PathData);
                Rings.RemoveAt(DataGrid.SelectedIndex);
                DataGrid.Items.Refresh();
            }
        }
    }
}
