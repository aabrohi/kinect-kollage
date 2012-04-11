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
using System.Windows.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;

namespace KinectKollagePhoneApp
{
    public partial class HorizTextPage : PhoneApplicationPage
    {
        private Point currentPoint;
        SolidColorBrush colorText;
        FontFamily font;
        int fsize;
        string fontName = "";
        string fontColor = "";
        string text = "";
        public HorizTextPage()
        {
            InitializeComponent();
            BitmapImage bi = new BitmapImage();
            //this.ContentPanelCanvas.MouseMove += new MouseEventHandler(ContentPanelCanvas_MouseMove);
            this.ContentPanelCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(ContentPanelCanvas_MouseLeftButtonDown);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = store.OpenFile("tempJPEG2", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    bi.SetSource(fileStream);
                    image2.Source = bi;
                }
            }

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

        private void colorBox(string data)
        {
            //string data = (sender as ListBox).SelectedItem as string;
            if (data == "Black")
                colorText = new SolidColorBrush(Colors.Black);
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

        private void fontBox(string data)
        {
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

        /*
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
        */

        void ContentPanelCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int fontsize = fsize*3;
            currentPoint = e.GetPosition(ContentPanelCanvas);
            // oldPoint = currentPoint;
            //ValidateText();
            TextBlock t = new TextBlock();
            /*if (String.IsNullOrWhiteSpace(textsize.Text))
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
            */
            colorBox(fontColor);
            fontBox(fontName);
            t.FontFamily = font;
            t.Foreground = colorText;
            t.FontSize = fontsize;
            t.Text = text;
            Canvas.SetTop(t, currentPoint.Y);
            Canvas.SetLeft(t, currentPoint.X);
            this.ContentPanelCanvas.Children.Add(t);
        }
        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            //MessageBox.Show(this.Orientation.ToString());
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (myStore.FileExists("tempJPEG2"))
                {
                    myStore.DeleteFile("tempJPEG2");
                }

                IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG2");

                WriteableBitmap wb = new WriteableBitmap(ContentPanelCanvas, null);
                wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
                myFileStream.Close();

                NavigationService.Navigate(new Uri("/TextPage.xaml", UriKind.Relative));
            }
            else
            {

            }
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string temp = NavigationContext.QueryString["text"];
            text = temp;
            //MessageBox.Show(text);
            temp = NavigationContext.QueryString["color"];
            fontColor = temp;
            temp = NavigationContext.QueryString["name"];
            fontName = temp;
            temp = NavigationContext.QueryString["size"];
            fsize = Convert.ToInt32(temp);
        }
       
    }
}