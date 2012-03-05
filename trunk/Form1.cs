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

namespace UDPSocketImageReceiverForm
{
    public partial class Form1 : Form
    {

        //UdpClient udpc;
        //IPEndPoint ep;
        Socket server;
        //EndPoint Remote;
        //IPEndPoint ipep;

        public Form1() //Initialize GUI
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
            //Debug.WriteLine("Test");
        }

        byte[] byteData = new byte[10485760]; // btw 10 485 760 = 10 mb



        private void Form1_Load(object sender, EventArgs e) // Load after Form1
        {
             try
             {
                 //We are using UDP sockets
                 server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);
                 
                 //Assign the any IP of the machine and listen on port number 7070
                 IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 7070);

                 //Bind this address to the server
                 server.Bind(ipEndPoint);

                 IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
                 //The epSender identifies the incoming clients
                 EndPoint epSender = (EndPoint)ipeSender;

                 Debug.WriteLine("Did you get this far?");

                 //Start receiving data
                 server.BeginReceiveFrom(byteData, 0, byteData.Length,
                     SocketFlags.None, ref epSender,
                        new AsyncCallback(OnReceive), epSender);
                
                  //if(byteData.Length != 0)
                    
                     Debug.WriteLine("The value of byteData is" + byteData);
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message, "UDP Server could not Initialize",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }

        private void OnReceive(IAsyncResult ar)
        {
            Debug.WriteLine("I got some stuff yo"); // Output this to the debug window if data is received

            StreamReader streamReader = new StreamReader("C:/hd-x-sign.txt");
            string text = streamReader.ReadToEnd();


            string stringnowbuffer = byteData.ToString();

            byte[] imageBytes = Convert.FromBase64String(text);

            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            pictureBox1.Image = image;
            pictureBox1.Show();

            //MemoryStream ms = new MemoryStream(byteData, 0,
            //   byteData.Length);

            // Convert byte[] to Image
            //ms.Write(byteData, 0, byteData.Length);
            //Image image = Image.FromStream(ms, true); // TODO: THIS IS TROUBLE, FIX THIS!!! Need to convert data from the stream to an Image
                     
            
            //disp(); // Move the Image object into here after you fix the debug up above
        }

     /*   private void button1_Click(object sender1, EventArgs e) // You there?
        {
            byte[] data = new byte[1024];
            //string input, stringData;
            ipep = new IPEndPoint(IPAddress.Parse("192.168.1.131"), 17);

            server = new Socket(AddressFamily.InterNetwork,
                          SocketType.Dgram, ProtocolType.Udp);


            string welcome = "Hello, are you there?";
            data = Encoding.ASCII.GetBytes(welcome);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);

            //server.Receive

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            Remote = (EndPoint)sender;

            //disp(); //breaks program otherwise, unnecessary :)

            server.Close();

        }
*/        //-----------Displaying the image------
        public void disp() // TODO: Take the data and put it back into an image, and then display it in the Form for all to see
        {

            //byte[] imageBytes = Convert.FromBase64String(base64String);
           // byte[] data = new byte[102400];
            //int recv = server.ReceiveFrom(byteData, ref Remote)
           
         //   Image returnImage = Image.FromStream(ms);

            //pictureBox1.Image = image;
            //pictureBox1.Show();

            Debug.WriteLine("omg i am making a picture!!!");

            byteData = Encoding.ASCII.GetBytes("OK");
            //server.SendTo(byteData, byteData.Length, SocketFlags.None, ipep);
           disp();
     

        }

   /*     private void button1_Click_1(object sender, EventArgs e)
        {

        }
    */ //don't need
    }
}
