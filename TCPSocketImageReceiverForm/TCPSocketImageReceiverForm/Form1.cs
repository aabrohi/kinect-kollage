using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Diagnostics; // for diagnosing, HOUSE MD style :P
using System.Windows;
using System.Threading;



namespace TCPSocketImageReceiverForm
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 10485760; //10mb
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public partial class Form1 : Form
    {

        //UdpClient udpc;
        //IPEndPoint ep;
        //Socket s;
        //EndPoint Remote;
        //IPEndPoint ipep;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public Form1() //Initialize GUI
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
            //Debug.WriteLine("Test");
        }

        // byte[] byteData = new byte[10485760]; // btw 10 485 760 = 10 mb



        private void Form1_Load(object sender, EventArgs e) // Load after Form1
        {
            // Data buffer for incoming data.
            //   byte[] bytes = new Byte[10485760];

            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 7070);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);



            Debug.WriteLine("First part of the buffer: " + state.buffer[0]);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();

                if (content.IndexOf("<EOF>") > -1)
                {
                    Debug.WriteLine("Content is: " + content);
                    // All the data has been read from the  
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n ",
                        content.Length);

                    //                    Debug.WriteLine("For Brandon: " + modcontent);



                    // Echo the data back to the client.
                    //                 Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }

            byte[] imageBytes = Convert.FromBase64String(content);

            FileStream fs = File.Create(@"C:\Users\ddsniper\Downloads\1.jpg");
            fs.Write(imageBytes, 0, imageBytes.Length);
            fs.Close();
        }

        /*        private void OnReceive(IAsyncResult ar)
                {
                    Debug.WriteLine("I got some stuff yo"); // Output this to the debug window if data is received

                    FileStream fs = File.Create(@"C:\Users\ddsniper\Downloads\1.jpg");
                    fs.Write(byteData, 0, byteData.Length);

                    Image image = null;
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        image = Image.FromStream(stream);
                    }

                    pictureBox1.Image = new Bitmap(image);
                    pictureBox1.Show();

                }
        */
    }
}
