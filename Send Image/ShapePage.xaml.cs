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

namespace Send_Image
{
    public partial class ShapePage : PhoneApplicationPage
    {
        private Point currentPoint;
        private Point oldPoint;
        public ShapePage()
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

        void ContentPanelCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(ContentPanelCanvas);
            oldPoint = currentPoint;
        }

        void ContentPanelCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            currentPoint = e.GetPosition(this.ContentPanelCanvas);

            Line line = new Line() { X1 = currentPoint.X, Y1 = currentPoint.Y, X2 = oldPoint.X, Y2 = oldPoint.Y };

            line.Stroke = new SolidColorBrush(Colors.Purple);
            line.StrokeThickness = 15;

            this.ContentPanelCanvas.Children.Add(line);
            oldPoint = currentPoint;

            //Line line = new Line();
            //Point point1 = new Point();
            //point1.X = 10.0;
            //point1.Y = 100.0;

            //Point point2 = new Point();
            //point2.X = 150.0;
            //point2.Y = 100.0;

            //line.X1 = point1.X;
            //line.Y1 = point1.Y;
            //line.X2 = point2.X;
            //line.Y2 = point2.Y;
        }
       
    }
}