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
//using System.Object;
using System.Windows.Media.Imaging;

namespace UDPSocketUI
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constants
        const int IMAGE_PORT = 7070;  // The Image will be sent on port 1400, since this is unassigned

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
                // Instantiate the UDPSocket
                SocketClient client = new SocketClient();

                string sendthatimage = image1.ToString();

                // Attempt to send our message to the server
                string result = client.Send(txtRemoteHost.Text, IMAGE_PORT, sendthatimage); //need to fix this...

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

/*
        /// <summary>
        /// Handle the btnEcho_Click event by sending text to the echo server and outputting the response
        /// </summary>
        private void btnEcho_Click(object sender, RoutedEventArgs e)
        {
            // Clear the log 
            ClearLog();

            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost() && ValidateInput())
            {
                // Instantiate the SocketClient
                SocketClient client = new SocketClient();

                // Attempt to send our message to be echoed to the echo server
                Log(String.Format("Sending '{0}' to server ...", txtInput.Text), true);
                string result = client.Send(txtRemoteHost.Text, ECHO_PORT, txtInput.Text);
                Log(result, false);

                // Receive a response from the echo server
                Log("Requesting Receive ...", true);
                result = client.Receive(ECHO_PORT);
                Log(result, false);

                // Close the socket connection explicitly
                client.Close();
            }

        }

        /// <summary>
        /// Handle the btnEcho_Click event by receiving text from the Quote of the Day (QOTD) server and outputting the response
        /// </summary>
        private void btnGetQuote_Click(object sender, RoutedEventArgs e)
        {
            // Clear the log 
            ClearLog();

            // Make sure we can perform this action with valid data
            if (ValidateRemoteHost())
            {
                // Instantiate the SocketClient object
                SocketClient client = new SocketClient();

                // Quote of the Day (QOTD) sends a short message (selected by the server’s administrator) to a client device. 
                // For UDP, the message is sent for each incoming UDP message, so here we send a "dummy" message to solicit 
                // a response. The message cannot be empty, so the message below consists of one whitespace.
                Log(String.Format("Requesting a quote from server '{0}' ...", txtInput.Text), true);
                string dummyMessage = " ";
                string result = client.Send(txtRemoteHost.Text, QOTD_PORT, dummyMessage);
                Log(result, false);

                // Receive response from the QOTD server
                Log("Requesting Receive ...", true);
                result = client.Receive(QOTD_PORT);
                Log(result, false);

                // Close the socket connection explicitly
                client.Close();
            }
        }

        #region UI Validation
        /// <summary>
        /// Validates the txtInput TextBox
        /// </summary>
        /// <returns>True if the txtInput TextBox contains valid data, False otherwise</returns>
        private bool ValidateInput()
        {
            // txtInput must contain some text
            if (String.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Please enter some text to echo");
                return false;
            }

            return true;
        }

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

        #region Logging
        /// <summary>
        /// Log text to the txtOutput TextBox
        /// </summary>
        /// <param name="message">The message to write to the txtOutput TextBox</param>
        /// <param name="isOutgoing">True if the message is an outgoing (client to server) message, False otherwise</param>
        /// <remarks>We differentiate between a message from the client and server 
        /// by prepending each line  with ">>" and "<<" respectively.</remarks>
        private void Log(string message, bool isOutgoing)
        {
            string direction = (isOutgoing) ? ">> " : "<< ";
            txtOutput.Text += Environment.NewLine + direction + message;
        }

        /// <summary>
        /// Clears the txtOutput TextBox
        /// </summary>
        private void ClearLog()
        {
            txtOutput.Text = String.Empty;
        }
        #endregion
    }
}
*/