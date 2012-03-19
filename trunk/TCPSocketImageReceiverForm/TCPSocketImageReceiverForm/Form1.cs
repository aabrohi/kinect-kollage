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

using System.Web;
using TwiPLi;
using System.Xml.Linq;

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

        public int picCounter = 0;
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



          //  Debug.WriteLine("First part of the buffer: " + state.buffer[0]);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            Random random = new Random();

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

            int number = random.Next(0, 100);
            string thefilename = "C:\\Users\\ddsniper\\Downloads\\CCT.NUI (12689)\\Images\\" + "picture_" + number + ".jpg";
            Debug.WriteLine("Writing the picture to " + thefilename);
            FileStream fs = File.Create(@thefilename);
            fs.Write(imageBytes, 0, imageBytes.Length);

            TwitPic tw = new TwitPic();
            Debug.WriteLine("image/png " + "image_ " + number + " " + thefilename + " " + "f1df26cd49afe58d92fff17cdd1c94bf " + "494611009-ybbVnZ9ThmVUhN65QvH0x2l48BcXUtF0pNG8AUq4 " + "YZ2eGUbGnYz1ratWweQD1fpK1JuxAUtJ4nIZBA1Y " + "cflG9inzlNltp2Znw5zEWA " + "CDS4bQi9NRdRRBGR4Am1skJNRonHbGrsBFwUmpk ");
            string upload_script = tw.UploadPhoto(imageBytes, "image/png", "image_" + number, thefilename, "f1df26cd49afe58d92fff17cdd1c94bf", "494611009-ybbVnZ9ThmVUhN65QvH0x2l48BcXUtF0pNG8AUq4", "YZ2eGUbGnYz1ratWweQD1fpK1JuxAUtJ4nIZBA1Y", "cflG9inzlNltp2Znw5zEWA", "CDS4bQi9NRdRRBGR4Am1skJNRonHbGrsBFwUmpk").ToString();
            Debug.WriteLine("Result of upload_script =" + upload_script);
            fs.Close();
        }
        public class TwitPic
        {
            private const string TWITPIC_UPLADO_API_URL = "http://api.twitpic.com/2/upload";
            private const string TWITPIC_UPLOAD_AND_POST_API_URL = "http://api.twitpic.com/1/uploadAndPost.xml";
            /// 
            /// Uploads the photo and sends a new Tweet
            /// 
            /// <param name="binaryImageData">The binary image data.
            /// <param name="tweetMessage">The tweet message.
            /// <param name="filename">The filename.
            /// Return true, if the operation was succeded.
            public string UploadPhoto(byte[] binaryImageData, string ContentType, string tweetMessage, string filename, string tpkey, string usrtoken, string usrsecret, string contoken, string consecret)
            {
                string boundary = Guid.NewGuid().ToString();
                string requestUrl = String.IsNullOrEmpty(tweetMessage) ? TWITPIC_UPLADO_API_URL : TWITPIC_UPLOAD_AND_POST_API_URL;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                string encoding = "iso-8859-1";

                request.PreAuthenticate = true;
                request.AllowWriteStreamBuffering = true;
                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                request.Method = "POST";

                string header = string.Format("--{0}", boundary);
                string footer = string.Format("--{0}--", boundary);

                StringBuilder contents = new StringBuilder();
                contents.AppendLine(header);

                string fileContentType = ContentType;
                string fileHeader = String.Format("Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"", "media", filename);
                string fileData = Encoding.GetEncoding(encoding).GetString(binaryImageData);

                contents.AppendLine(fileHeader);
                contents.AppendLine(String.Format("Content-Type: {0}", fileContentType));
                contents.AppendLine();
                contents.AppendLine(fileData);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "key"));
                contents.AppendLine();
                contents.AppendLine(tpkey);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "consumer_token"));
                contents.AppendLine();
                contents.AppendLine(contoken);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "consumer_secret"));
                contents.AppendLine();
                contents.AppendLine(consecret);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "oauth_token"));
                contents.AppendLine();
                contents.AppendLine(usrtoken);

                contents.AppendLine(header);
                contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "oauth_secret"));
                contents.AppendLine();
                contents.AppendLine(usrsecret);

                if (!String.IsNullOrEmpty(tweetMessage))
                {
                    contents.AppendLine(header);
                    contents.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "message"));
                    contents.AppendLine();
                    contents.AppendLine(tweetMessage);
                }

                contents.AppendLine(footer);
                byte[] bytes = Encoding.GetEncoding(encoding).GetBytes(contents.ToString());
                request.ContentLength = bytes.Length;

                string mediaurl = "";
                try
                {
                    using (Stream requestStream = request.GetRequestStream()) // this is where the bug is due to not being able to seek.
                    {
                        requestStream.Write(bytes, 0, bytes.Length); // No problem the image is posted and tweet is posted
                        requestStream.Close();
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) // here I can't get the response
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                string result = reader.ReadToEnd();

                                XDocument doc = XDocument.Parse(result); // this shows no root elements and fails here
                // taken from http://stackoverflow.com/questions/3338837/porting-the-twitpic-api-curl-example-to-c-multipart-data
                           //     XElement rsp = doc.Element("rsp");
                           //     string status = rsp.Attribute(XName.Get("status")) != null ? rsp.Attribute(XName.Get("status")).Value : rsp.Attribute(XName.Get("stat")).Value;
                           //     mediaurl = rsp.Element("mediaurl").Value;
                                mediaurl = doc.Element("image").Element("url").Value;
                                return mediaurl;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                return mediaurl;
            }
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
