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
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;

namespace KinectKollagePhoneApp
{
    public partial class PenPage : PhoneApplicationPage
    {
        private Point currentPoint;
        private Point oldPoint;
        //WriteableBitmap writeableBitmapImg;

        public PenPage()
        {
            InitializeComponent();

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                img.Source = imageAsBitmap;
                //img.Source = imageAsBitmap;

            }
            //writeableBitmapImg.Invalidate();
            this.imageCanvas.MouseMove += new MouseEventHandler(img_OnMouseMove);
            this.imageCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(img_OnMouseLeftButtonDown);

        }

        void img_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(imageCanvas);
            oldPoint = currentPoint;
        }

        //void img_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    writeableBitmapImg.Invalidate();
        //}

        void img_OnMouseMove(object Sender, MouseEventArgs e)
        {
            currentPoint = e.GetPosition(imageCanvas);
            //writeableBitmapImg.DrawLine((int)currentPoint.X, (int)currentPoint.Y, (int)oldPoint.X, (int)oldPoint.Y, Colors.Purple);
            //writeableBitmapImg.Invalidate();
            Line line = new Line() { X1 = currentPoint.X, Y1 = currentPoint.Y, X2 = oldPoint.X, Y2 = oldPoint.Y };
            line.Stroke = new SolidColorBrush(Colors.Purple);
            line.StrokeThickness = 15;
            this.imageCanvas.Children.Add(line);
            oldPoint = currentPoint;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //BinaryReader objReader = new BinaryReader(writeableBitmapImg.ToByteArray);
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG");

            WriteableBitmap wb = new WriteableBitmap(imageCanvas, null);
            wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
            myFileStream.Close();

            myFileStream = myStore.OpenFile("tempJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture("NewPicture.jpg", myFileStream);
            //MessageBox.Show("Image saved");
            myFileStream.Close();

            // Save to image.jpg
            //WriteableBitmap bmp = new WriteableBitmap(img,null);
            //bmp.SaveJpeg(local,,,0,100);

        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditPage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                img.Source = imageAsBitmap;
            }
        }
    }
}