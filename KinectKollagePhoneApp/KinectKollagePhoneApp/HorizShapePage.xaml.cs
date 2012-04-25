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
    public partial class HorizShapePage : PhoneApplicationPage
    {
        private int stickerNum;
        private Point currentPoint;
        private Point oldPoint;
        public HorizShapePage()
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
        }

        private void flower_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 1;
        }
        private void heart_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 2;
        }
        private void smiley_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 3;
        }
        private void snowman_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 4;
        }
        private void star_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 5;
        }
        private void balloon_Click(object sender, RoutedEventArgs e)
        {
            stickerNum = 6;
        }

        void ContentPanelCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(ContentPanelCanvas);
            oldPoint = currentPoint;
            switch (stickerNum)
            {
                case 1:
                    Image temp1 = new Image();
                    currentPoint.X -= 30;
                    currentPoint.Y -= 30;
                    temp1.Source = new BitmapImage(new Uri("newflower.png", UriKind.RelativeOrAbsolute));
                    temp1.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp1);
                    break;
                case 2:
                    Image temp2 = new Image();
                    currentPoint.X -= 30;
                    currentPoint.Y -= 29;
                    temp2.Source = new BitmapImage(new Uri("newheart.png", UriKind.RelativeOrAbsolute));
                    temp2.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp2);
                    break;
                case 3:
                    Image temp3 = new Image();
                    currentPoint.X -= 30;
                    currentPoint.Y -= 30;
                    temp3.Source = new BitmapImage(new Uri("newsmiley.png", UriKind.RelativeOrAbsolute));
                    temp3.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp3);
                    break;
                case 4:
                    Image temp4 = new Image();
                    currentPoint.X -= 30;
                    currentPoint.Y -= 36;
                    temp4.Source = new BitmapImage(new Uri("newsnowman.png", UriKind.RelativeOrAbsolute));
                    temp4.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp4);
                    break;
                case 5:
                    Image temp5 = new Image();
                    currentPoint.X -= 30;
                    currentPoint.Y -= 29;
                    temp5.Source = new BitmapImage(new Uri("newstar.png", UriKind.RelativeOrAbsolute));
                    temp5.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp5);
                    break;
                case 6:
                    Image temp6 = new Image();
                    currentPoint.X -= 31;
                    currentPoint.Y -= 41;
                    temp6.Source = new BitmapImage(new Uri("newballoon.png", UriKind.RelativeOrAbsolute));
                    temp6.Margin = new Thickness(currentPoint.X, currentPoint.Y, 0, 0);
                    this.ContentPanelCanvas.Children.Add(temp6);
                    break;
            }
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

                NavigationService.Navigate(new Uri("/ShapePage.xaml", UriKind.Relative));
            }
            else
            {

            }
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string temp = NavigationContext.QueryString["sn"];
            stickerNum = Convert.ToInt32(temp);
            //MessageBox.Show(e.Content.ToString());
            //this.ContentPanelCanvas = (e.Content as ShapePage).ContentPanelCanvas;
            //MessageBox.Show(temp);
        }
       
    }
}