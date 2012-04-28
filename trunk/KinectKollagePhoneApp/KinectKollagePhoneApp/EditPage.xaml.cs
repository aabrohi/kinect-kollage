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
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;

namespace KinectKollagePhoneApp
{
    public partial class EditPage : PhoneApplicationPage
    {
        int PenInst;
        int StickInst;
        int TextInst;

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

        /*private void backButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        */
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(StickInst.ToString());
            string url = "/ShapePage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/ShapePage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string url = "/PenPage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/PenPage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string url = "/TextPage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/TextPage.xaml", UriKind.Relative));
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

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            string url = "/MainPage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            e.Cancel = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string temp = NavigationContext.QueryString["PI"];
            PenInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["SI"];
            StickInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["TI"];
            TextInst = Convert.ToInt32(temp);
            //MessageBox.Show(e.Content.ToString());
            //this.ContentPanelCanvas = (e.Content as ShapePage).ContentPanelCanvas;
            //MessageBox.Show(temp);
        }
    }
}