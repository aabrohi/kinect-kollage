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

using System.Net.Sockets;
using System.Text;

namespace Send_Image
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constants
        const int IMAGE_PORT = 17;  // The Echo protocol uses port 14, since this is unassigned

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
                // Convert image to WriteableBitmap
                WriteableBitmap bmp = new WriteableBitmap(image1, null);
                string chosenPhoto = bmp.ToString();
               
                //// Convert to byte array
                //int[] p = bmp.Pixels;
                //int len = p.Length * 4;
                //byte[] imageByteArray = new byte[len]; //ARGB array
                //Buffer.BlockCopy(p, 0, imageByteArray, 0, len);
                //string chosenPhoto = Convert.ToBase64String(imageByteArray);

                // Instantiate the UDPSocket
                UDPSocket client = new UDPSocket();

                // Attempt to send our message to the server
                string result = client.Send(txtRemoteHost.Text, IMAGE_PORT, chosenPhoto); //need to fix this...

                // Receive a response from the server
                result = client.Receive(IMAGE_PORT);

                // Close the socket connection explicitly
                client.Close();
            }
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
    }
}