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
using Coding4Fun.Phone.Controls;


namespace Send_Image
{
    public partial class TextPage : PhoneApplicationPage
    {
        private Point currentPoint;
        private Point oldPoint;
        public TextPage()
        {
            InitializeComponent();
            this.ContentPanelCanvas.MouseMove += new MouseEventHandler(ContentPanelCanvas_MouseMove);
            this.ContentPanelCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(ContentPanelCanvas_MouseLeftButtonDown);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                image2.Source = imageAsBitmap;
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
            wb.SaveJpeg(myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
            myFileStream.Close();

            myFileStream = myStore.OpenFile("tempJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture("NewPicture.jpg", myFileStream);
            //MessageBox.Show("Image saved");
            myFileStream.Close();
            
            //NavigationService.Navigate(new Uri("/SavePage.xaml", UriKind.Relative));
        }

        private void colorButton1_Click(object sender, RoutedEventArgs e)
        {
           // ColorSlider slider = new ColorSlider();
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
            oldPoint = currentPoint;
            ValidateText();
            TextBlock t = new TextBlock();
            if (String.IsNullOrWhiteSpace(textsize.Text))
            {
                fontsize = 16;
            }
          
           // else fontsize = textsize.Text;
            t.FontSize = fontsize;
            t.Text = enteredtxt.Text.ToString();
            Canvas.SetTop(t, currentPoint.Y);
            Canvas.SetLeft(t, currentPoint.X);
            this.ContentPanelCanvas.Children.Add(t);
        }

        void ContentPanelCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            currentPoint = e.GetPosition(this.ContentPanelCanvas);

            Line line = new Line() { X1 = currentPoint.X, Y1 = currentPoint.Y, X2 = oldPoint.X, Y2 = oldPoint.Y };

            line.Stroke = new SolidColorBrush(Colors.Purple);
            line.StrokeThickness = 15;

            this.ContentPanelCanvas.Children.Add(line);
            oldPoint = currentPoint;

        }
       
    }
}