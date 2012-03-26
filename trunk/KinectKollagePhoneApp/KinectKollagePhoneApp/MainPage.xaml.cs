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

        String image2;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
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
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(e.ChosenPhoto);
                    Uri uri = new Uri(e.OriginalFileName);
                    image2 = e.OriginalFileName;
                    Debug.WriteLine(image2);
                    break;
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
            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost())
            {

                // Instantiate the TCPSocket
                SocketClient client = new SocketClient();
                string result = client.Connect(txtRemoteHost.Text, IMAGE_PORT);

                //         result = client.Send(Encoding.UTF8.GetBytes(message));
                string base64String = Convert.ToBase64String(ReadImageFile(image2));
                //Debug.WriteLine("I'm sending this: " + base64String);
                Debug.WriteLine("First part of the buffer: " + Encoding.UTF8.GetBytes(base64String + "<EOF>").Length);
                result = client.Send(Encoding.UTF8.GetBytes(base64String));
                Console.WriteLine("Send success!");

                // Close the socket connection explicitly
                //                client.Close();

                ///////////////////////////////////////////////////////////////
                // Convert image to WriteableBitmap
            }
        }

        private static byte[] ReadImageFile(String img)
        {

            Uri jpegUri = new Uri(img, UriKind.Relative);
            StreamResourceInfo sri = Application.GetResourceStream(jpegUri);

            byte[] buf = new byte[sri.Stream.Length];       //need to create a new buf everytime to account for varying image sizes
            //remedy: do not upload the same picture twice in a row/be a David
            //how to fix later... clear out the image1 from the screen everytime you click upload
            sri.Stream.Read(buf, 0, buf.Length);

            return buf;
        }

        #region UI Validation
        /// <summary>
        /// Validates the txtRemoteHost TextBox
        /// </summary>
        /// <returns>True if the txtRemoteHost contains valid data, False otherwise</returns>

        private bool ValidateRemoteHost()
        {
            // The txtRemoteHost must contain some text
            if (String.IsNullOrWhiteSpace(txtRemoteHost.Text))
            {
                MessageBox.Show("Please enter a host name");
                return false;
            }

            return true;
        }
        #endregion

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditPage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                image1.Source = imageAsBitmap;
            }
        }
    }
}