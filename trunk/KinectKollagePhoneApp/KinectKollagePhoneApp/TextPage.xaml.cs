using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Resources;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;



namespace KinectKollagePhoneApp
{
    public partial class TextPage : PhoneApplicationPage
    {
        private Point currentPoint;
        private Point oldPoint;
        SolidColorBrush colorText;
        FontFamily font;
        public TextPage()
        {
            InitializeComponent();
            // this.ContentPanelCanvas.MouseMove += new MouseEventHandler(ContentPanelCanvas_MouseMove);
           this.ContentPanelCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(ContentPanelCanvas_MouseLeftButtonDown);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                image2.Source = imageAsBitmap;
            }

            this.colorBox.Items.Add("Black");
            this.colorBox.Items.Add("White");
            this.colorBox.Items.Add("Blue");
            this.colorBox.Items.Add("Green");
            this.colorBox.Items.Add("Red");
            this.colorBox.Items.Add("Yellow");
            this.colorBox.Items.Add("Purple");

            this.fontBox.Items.Add("Arial");
            this.fontBox.Items.Add("Century Gothic");
            this.fontBox.Items.Add("Comic Sans");
            this.fontBox.Items.Add("Tacoma");
            this.fontBox.Items.Add("Times New Roman");
            this.fontBox.Items.Add("Verdana");

        }


        private void backButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditPage.xaml", UriKind.Relative));
        }

        private void saveButton1_Click(object sender, RoutedEventArgs e)
        {
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG");

            WriteableBitmap wb = new WriteableBitmap(ContentPanelCanvas, null);
            wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
            myFileStream.Close();

            myFileStream = myStore.OpenFile("tempJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture("NewPicture.jpg", myFileStream);
            MessageBox.Show("Image saved");
            myFileStream.Close();
            
            //NavigationService.Navigate(new Uri("/SavePage.xaml", UriKind.Relative));
        }

        private void colorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string data = (sender as ListBox).SelectedItem as string;
            if (data == "White")
                colorText = new SolidColorBrush(Colors.White);
            else if (data == "White")
                colorText = new SolidColorBrush(Colors.White);
            else if (data == "Blue")
                colorText = new SolidColorBrush(Colors.Blue);
            else if (data == "Green")
                colorText = new SolidColorBrush(Colors.Green);
            else if (data == "Red")
                colorText = new SolidColorBrush(Colors.Red);
            else if (data == "Yellow")
                colorText = new SolidColorBrush(Colors.Yellow);
            else if (data == "Purple")
                colorText = new SolidColorBrush(Colors.Purple);

        }

        private void fontBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string data = (sender as ListBox).SelectedItem as string;
            if (data == "Arial")
                font = new FontFamily("Arial");
            else if (data == "Century Gothic")
                font = new FontFamily("Century Gothic");
            else if (data == "Comic Sans")
                font = new FontFamily("Comic Sans");
            else if (data == "Tacoma")
                font = new FontFamily("Tacoma");
            else if (data == "Times New Roman")
                font = new FontFamily("Times New Roman");
            else if (data == "Verdana")
                font = new FontFamily("Verdana");
        }


        private bool ValidateText()
        {
            // The txtRemoteHost must contain some text
            if (String.IsNullOrWhiteSpace(enteredtxt.Text))
            {
                MessageBox.Show("Please enter text");
                return false;
            }

            return true;
        }

        
        void ContentPanelCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int fontsize = 16;
            currentPoint = e.GetPosition(ContentPanelCanvas);
           // oldPoint = currentPoint;
            ValidateText();
            TextBlock t = new TextBlock();
            if (String.IsNullOrWhiteSpace(textsize.Text))
            {
                fontsize = 16;
            }
          
            else
            {
                string tsize = textsize.Text.ToString();
                try
                {
                    fontsize = Int32.Parse(tsize);
                }

                catch (FormatException)
                {
                    fontsize = 16;
                }

            }

            t.FontFamily = font;
            t.Foreground = colorText;
            t.FontSize = fontsize;
            t.Text = enteredtxt.Text.ToString();
            Canvas.SetTop(t, currentPoint.Y);
            Canvas.SetLeft(t, currentPoint.X);
            this.ContentPanelCanvas.Children.Add(t);
        }
        /*
       void ContentPanelCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            //currentPoint = e.GetPosition(this.ContentPanelCanvas);

            TextBlock t = new TextBlock();
            t.Text = enteredtxt.Text.ToString();
            TextBlock tp = t;
            this.ContentPanelCanvas.Children.Remove(tp);
            //Canvas.SetTop(t, currentPoint.Y);
            //Canvas.SetLeft(t, currentPoint.X);
            this.ContentPanelCanvas.Children.Add(t);

        }   
       */
    }
}