/* by Markus */

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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using System.IO;
using System.IO.IsolatedStorage;

using System.Net.Sockets;
using System.Text;

using System.Diagnostics;
using System.Windows.Resources;

namespace KinectKollagePhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constants
        const int IMAGE_PORT = 7070;  // The Echo protocol uses port 14, since this is unassigned


        // Constructor
        public MainPage()
        {
            InitializeComponent();
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists("tempJPEG2"))
            {
                myStore.DeleteFile("tempJPEG2");
            }
        }
        void button1_Click(object sender, RoutedEventArgs e)
        {
            image1.Source = null;
            PhotoChooserTask objPhotoChooser = new PhotoChooserTask();
            objPhotoChooser.Completed += new EventHandler<PhotoResult>(PhotoChooseCall);
            objPhotoChooser.Show();
        }
        void PhotoChooseCall(object sender, PhotoResult e)
        {
            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    BinaryReader objReader = new BinaryReader(e.ChosenPhoto);
                    image1.Source = new BitmapImage(new Uri(e.OriginalFileName));
                    // New code
                    var contents = new byte[1024];
                    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var local = new IsolatedStorageFileStream("image.jpg", FileMode.OpenOrCreate, store))
                        {
                            int bytes;
                            while ((bytes = e.ChosenPhoto.Read(contents, 0, contents.Length)) > 0)
                            {
                                local.Write(contents, 0, bytes);
                            }
                        }
                    }
                    break;
                //    BitmapImage bi = new BitmapImage();
                //    bi.SetSource(e.ChosenPhoto);
                //    Uri uri = new Uri(e.OriginalFileName);
                //    image2 = e.OriginalFileName;
                //    Debug.WriteLine(image2);
                //    break;
                case TaskResult.Cancel:
                    MessageBox.Show("Cancelled");
                    break;
                case TaskResult.None:
                    MessageBox.Show("Nothing Entered");
                    break;
            }
        }

        private void upload1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UploadPage.xaml", UriKind.Relative));
        }

        //private static byte[] ReadImageFile(String img)
        //{

        //    Uri jpegUri = new Uri(img, UriKind.Relative);
        //    StreamResourceInfo sri = Application.GetResourceStream(jpegUri);

        //    byte[] buf = new byte[sri.Stream.Length];       //need to create a new buf everytime to account for varying image sizes
        //    //remedy: do not upload the same picture twice in a row/be a David
        //    //how to fix later... clear out the image1 from the screen everytime you click upload
        //    sri.Stream.Read(buf, 0, buf.Length);

        //    return buf;
        //}

       

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditPage.xaml", UriKind.Relative));
        }

    }
}