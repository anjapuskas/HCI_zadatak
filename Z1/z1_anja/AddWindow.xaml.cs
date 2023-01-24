using System;
using Microsoft.Win32;
using System.Collections.Generic;
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
using Classes;
using System.IO;
using System.Text.RegularExpressions;

namespace z1_anja
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        bool addpicture = false;
        string path = "";
        int index;
        bool _noediting = true;
        int wordCounter = 0;
        bool _init = false;

        public AddWindow()
        {
            InitializeComponent(); //prikazuje sve sto se nalazi unutar window tagova, obavezan u svakom konstruktoru
            _init = true;

            FontSize.ItemsSource = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            FontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
        }

        public AddWindow(int index)  //vec je popunjen podacima pa je prosledjen index 
        {
            InitializeComponent();
            this.index = index;   //poklapa se sa indexom
            _noediting = false;
            _init = true;
            ButtonAdd.Content = "Edit Ring";
            FontSize.ItemsSource = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            FontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            TextBoxName.Text = MainWindow.Rings[index].Name;              //isti prozor kao Add samo popunjen jer cita tabelu
            TextBoxPrice.Text = MainWindow.Rings[index].Price.ToString();
            DatePicker.SelectedDate = MainWindow.Rings[index].Date;
            Picture.Source = new BitmapImage( new Uri(MainWindow.Rings[index].PathImage));

            string pathToRtb = "./rtb" + MainWindow.Rings[index].ID + ".rtf";

            TextRange range;
            FileStream fStream;
            range = new TextRange(RichTextBoxDetails.Document.ContentStart, RichTextBoxDetails.Document.ContentEnd);
            fStream = new FileStream(pathToRtb, FileMode.OpenOrCreate);
            range.Load(fStream, DataFormats.Rtf);
            fStream.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private bool Validate()
        {
            bool isValid = true;  

            if (TextBoxName.Text.Trim().Equals(""))
            {
                isValid = false;
                TextBoxName.BorderBrush = Brushes.Red;
                TextBoxName.BorderThickness = new Thickness(2);
                NameErrorLabel.Content = "Fill this field!";
            }
            else
            {
                TextBoxName.BorderBrush = Brushes.LightSlateGray;
                TextBoxName.BorderThickness = new Thickness(1);
                NameErrorLabel.Content = "";
            }

            if (TextBoxPrice.Text.Trim().Equals(""))
            {
                isValid = false;
                TextBoxPrice.BorderBrush = Brushes.Red;
                TextBoxPrice.BorderThickness = new Thickness(2);
                PriceErrorLabel.Content = "Fill this field!";
            }
            else
            {
                TextBoxPrice.BorderBrush = Brushes.LightSlateGray;
                PriceErrorLabel.Content = "";
                TextBoxPrice.BorderThickness = new Thickness(1);

                int i = 0;

                try
                {
                    i = Int32.Parse(TextBoxPrice.Text.Trim());
                }
                catch (Exception exc)
                {
                    TextBoxPrice.BorderBrush = Brushes.Red;
                    TextBoxPrice.BorderThickness = new Thickness(2);
                    PriceErrorLabel.Content = "Must be numbers!";
                    Console.WriteLine(exc.Message);
                    isValid = false;
                }


                if (i<0)
                {
                    isValid = false;
                    TextBoxPrice.BorderBrush = Brushes.Red;
                    TextBoxPrice.BorderThickness = new Thickness(2);
                    PriceErrorLabel.Content = "Must be number greater than 0!";
                }
                
            }


            if (wordCounter == 0)
            {
                DetailsErrorLabel.Content = "Fill this field!";
                isValid = false;
            }
            else
            {
                DetailsErrorLabel.Content = "";
            }


            if (DatePicker.SelectedDate is null)
            {
                DatePicker.BorderBrush = Brushes.Red;
                DatePicker.BorderThickness = new Thickness(2);
                DateErrorLabel.Content = "Fill this field!";
                isValid = false;
            }
            else
            {
                DateErrorLabel.Content = "";
                DatePicker.BorderBrush = Brushes.LightSlateGray;
                DatePicker.BorderThickness = new Thickness(1);
            }
              

            TextPointer startPointer = RichTextBoxDetails.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward); //za details
            TextPointer endPointer = RichTextBoxDetails.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            if (startPointer.CompareTo(endPointer) == 0)
            {
                RichTextBoxDetails.BorderBrush = Brushes.Red;
                RichTextBoxDetails.BorderThickness = new Thickness(2);
                DetailsErrorLabel.Content = "Fill this field!";
                isValid = false;
            }
            else
            {
                RichTextBoxDetails.BorderBrush = Brushes.LightSlateGray;
                DetailsErrorLabel.Content = "";
                RichTextBoxDetails.BorderThickness = new Thickness(1);
            }

            if (Picture.Source == null)
             {

                 PicturesErrorLabel.Content = "Fill this field!";
                 isValid = false;
             }
             else
                PicturesErrorLabel.Content = "";


             return isValid;
         }


            private void Button_Click_Browse(object sender, RoutedEventArgs e)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Select a picture";
                fileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

                if (fileDialog.ShowDialog() == true)
                {
                    Picture.Source = new BitmapImage(new Uri(fileDialog.FileName));
                    path = fileDialog.FileName;
                    addpicture = true;
                }
            }

        private void Button_Click_AddNew(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {

                if(_noediting)
                {
                    int id;

                    if (MainWindow.Rings.Count > 0) 
                    {
                        id = MainWindow.Rings[MainWindow.Rings.Count - 1].ID + 1;
                    }
                    else
                    {
                        id = 0;
                    }
                    string pathToRtb = "./rtb" + id + ".rtf";
                    TextRange range;
                    FileStream fStream;
                    range = new TextRange(RichTextBoxDetails.Document.ContentStart, RichTextBoxDetails.Document.ContentEnd);
                    fStream = new FileStream(pathToRtb, FileMode.Create);
                    range.Save(fStream, DataFormats.Rtf);
                    fStream.Close();
                    MainWindow.Rings.Add(new Ring(TextBoxName.Text, Convert.ToDouble(TextBoxPrice.Text), (DateTime)DatePicker.SelectedDate, path, pathToRtb, id));

                }   
                else
                {
                    int id = MainWindow.Rings[index].ID;
                    string pathToRtb = "./rtb" + id + ".rtf";
                    TextRange range;
                    FileStream fStream;
                    range = new TextRange(RichTextBoxDetails.Document.ContentStart, RichTextBoxDetails.Document.ContentEnd);
                    fStream = new FileStream(pathToRtb, FileMode.Create);
                    range.Save(fStream, DataFormats.Rtf);
                    fStream.Close();

                    MainWindow.Rings[index].Name = TextBoxName.Text;
                    MainWindow.Rings[index].Price = Convert.ToDouble(TextBoxPrice.Text);
                    MainWindow.Rings[index].Date = (DateTime)DatePicker.SelectedDate;

                    if (addpicture)
                    {
                        MainWindow.Rings[index].PathImage = path;
                    }
                }
                this.Close();

            }
            else
            {
                MessageBox.Show("Error: Please fill in all required fields.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }      
        }

        private void RichTextBoxDetails_SelectionChanged(object sender, RoutedEventArgs e)
        {

            object temp = RichTextBoxDetails.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldButton.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = RichTextBoxDetails.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicButton.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = RichTextBoxDetails.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = RichTextBoxDetails.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamily.SelectedItem = temp;

            temp = RichTextBoxDetails.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSize.SelectedItem = temp;

        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSize.SelectedItem != null && !RichTextBoxDetails.Selection.IsEmpty)
            {
                RichTextBoxDetails.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSize.SelectedItem);
            }

        }

        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamily.SelectedItem != null && !RichTextBoxDetails.Selection.IsEmpty)
            {
                RichTextBoxDetails.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamily.SelectedItem);
            }
        }

        private void Button_Color(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog color = new System.Windows.Forms.ColorDialog();
            if (color.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!RichTextBoxDetails.Selection.IsEmpty)
                {
                    RichTextBoxDetails.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(Color.FromArgb(color.Color.A, color.Color.R, color.Color.G, color.Color.B)));
                }
            }
        }


        private void RichTextBoxDetails_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(RichTextBoxDetails.Document.ContentStart, RichTextBoxDetails.Document.ContentEnd);
            if (_init)
            {
                wordCounter = Regex.Matches(textRange.Text, @"[\S]+").Count; // razmak 1 ili vise
                WordCounter.Text = "Word count: " + wordCounter.ToString();
            }
        }
    }
}
