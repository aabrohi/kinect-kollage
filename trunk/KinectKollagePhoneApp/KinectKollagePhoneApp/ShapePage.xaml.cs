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
using Microsoft.Xna.Framework; //////////////
using Microsoft.Devices.Sensors; ////////////

namespace KinectKollagePhoneApp
{
    public partial class ShapePage : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        double thresh = .5;
        bool undone = false;

        public int stickerNum { get; set; }

        public int PenInst;
        public int StickInst;
        public int TextInst;
        
        public ShapePage()
        {
            InitializeComponent();
            if (accelerometer == null)
            {
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
                accelerometer.Start();
            }
            BitmapImage bi = new BitmapImage();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                /*var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                image2.Source = imageAsBitmap;*/
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
            
            stickerNum = 0;
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => undo(e.SensorReading));
        }

        private void undo(AccelerometerReading acceleromterReading)
        {
            //MessageBox.Show("I shook the phone?");
            Vector3 acceleration = acceleromterReading.Acceleration;
            if ((acceleration.X > thresh || acceleration.Y > thresh || acceleration.Z > thresh) && !undone)
            {
                BitmapImage bi = new BitmapImage();
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fileStream = store.OpenFile("undoJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bi.SetSource(fileStream);
                        image2.Source = bi;
                        store.CopyFile("undoJPEG", "tempJPEG2", true);
                        fileStream.Close();
                    }
                }
                undone = true;
                //MessageBox.Show("I shook the phone?");
            }
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

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            //MessageBox.Show(this.Orientation.ToString());
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {

            }
            else
            {
                accelerometer.Stop();
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if(myStore.FileExists("tempJPEG2"))
                {
                    myStore.DeleteFile("tempJPEG2");
                }

                IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG2");

                WriteableBitmap wb = new WriteableBitmap(ContentPanelCanvas, null);
                wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
                myFileStream.Close();

                /////////////////////////////// UNDO
                if (myStore.FileExists("undoJPEG"))
                {
                    myStore.DeleteFile("undoJPEG");
                }

                IsolatedStorageFileStream myUndoStream = myStore.CreateFile("undoJPEG");

                WriteableBitmap uwb = new WriteableBitmap(ContentPanelCanvas, null);
                uwb.SaveJpeg(myUndoStream, 1000, 667, 0, 100);
                myUndoStream.Close();
                //////////////////////////////// \UNDO

                string url = "/HorizShapePage.xaml?sn=";
                url += stickerNum.ToString();
                url += "&PI=";
                url += PenInst.ToString();
                url += "&SI=";
                url += StickInst.ToString();
                url += "&TI=";
                url += TextInst.ToString();
                NavigationService.Navigate(new Uri(url, UriKind.Relative));
            }
        }
        
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //StickInst = 1;
            //MessageBox.Show(StickInst.ToString());
            string url = "/EditPage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            e.Cancel = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //MessageBox.Show("navigated to shapepage");
            string temp = NavigationContext.QueryString["PI"];
            PenInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["SI"];
            StickInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["TI"];
            TextInst = Convert.ToInt32(temp);

            if (StickInst == 0)
            {
                MessageBox.Show("Select a sticker, then rotate the phone horizontally to place as many of the selected sticker on the image as desired. "
                        + "Rotate the phone vertically to select a different sticker and/or return to the menu. Shake the phone when vertical to undo the previous edits.");
                StickInst = 1;
            }
            
            //MessageBox.Show(StickInst.ToString());

        }
    }
}