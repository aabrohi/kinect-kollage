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
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Net.Sockets;

using System.Diagnostics;
using System.Windows.Resources;


namespace TCPImageClient
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constants
        const int IMAGE_PORT = 7070;  // Use port 7070, not assigned to a TCP/UDP Standard

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
            if (ValidateRemoteHost())   //SET THIS TO TRUE IF YOU WANT TO BYPASS BLANK INPUT
            {
/*
        //        Socket sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress IP = IPAddress.Parse(txtRemoteHost.Text);
                IPEndPoint IPE = new IPEndPoint(IP, 7070);

        //        sListen.Bind(IPE); //WP 7.1 does not support this!

                Console.WriteLine("Service is listening ...");
        //        sListen.Listen(2); //WP 7.1 does not supprot this!

                while (true)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = sListen.Accept();
                    }
                    catch
                    {
                        throw;
                    }

                    // send data to the client
                    //clientSocket.Send (Encoding.Unicode.GetBytes ("I am a server, you there?? !!!!"));

                    // send the file
                    byte[] buffer = ReadImageFile("1.jpg");
                    clientSocket.Send(buffer, buffer.Length, SocketFlags.None);
                    Console.WriteLine("Send success!");
                }
*/

                // Instantiate the TCPSocket
                SocketClient client = new SocketClient();

                // Attempt to send our message to the server
                string result = client.Connect(txtRemoteHost.Text, IMAGE_PORT);

                // Receive a response from the server
//                result = client.Send();

                //string imageinstringformat = image2.Source;

                //string location = @"\" + image2;
                //Debug.WriteLine("Where is it? " + location);

                //Debug.WriteLine(image2.ToString());

                //byte[] buffer = ReadImageFile(image2);

       //string message = "hello";

                //ReadImageFile(image2);

       //         result = client.Send(Encoding.UTF8.GetBytes(message));
                string base64String = Convert.ToBase64String(ReadImageFile(image2));
                //Debug.WriteLine("I'm sending this: " + base64String);
                Debug.WriteLine("First part of the buffer: " + Encoding.UTF8.GetBytes(base64String + "<EOF>").Length);
                result = client.Send(Encoding.UTF8.GetBytes(base64String));
                Console.WriteLine("Send success!");

                // Close the socket connection explicitly
//                client.Close();
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
    }
}