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
using System.IO.IsolatedStorage;

namespace KinectKollagePhoneApp
{
    public partial class EditPage : PhoneApplicationPage
    {
        public EditPage()
        {
            InitializeComponent();
            BitmapImage bi = new BitmapImage();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                /*var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                image2.Source = imageAsBitmap;
                */
                if (store.FileExists("tempJPEG2"))
                {
                    using (IsolatedStorageFileStream fileStream = store.OpenFile("tempJPEG2", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bi.SetSource(fileStream);
                        image2.Source = bi;
                    }
                }
                else
                {
                    var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                    image2.Source = imageAsBitmap;
                }
            }

        }

        private void backButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ShapePage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PenPage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TextPage.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            e.Cancel = true;
        }

    }
}