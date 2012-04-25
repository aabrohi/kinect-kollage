using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using CCT.NUI.Visual;

using System.Runtime.InteropServices;

using System.Diagnostics; // for diagnosing, HOUSE MD style :P
using System.Windows;
using System.Threading;

using System.Web;
using System.Xml.Linq;

using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CCT.NUI.Samples.ImageManipulation
{
    public partial class ImageForm : Form
    {
        private IList<InteractiveImage> images = new List<InteractiveImage>();
        private IList<HandTracker> handTracks = new List<HandTracker>();

        private IHandDataSource handDataSource;
        private HandLayer handLayer;

        //These are variables that change per the computer that you are using the app on
        int reset_counter = 0;
        string thefilename = "C:\\Users\\dDSniper\\Downloads\\Screen\\test.jpg";
        string thefilename_res = "C:\\Users\\dDSniper\\Downloads\\Screen\\test300.jpg";

        public ImageForm()
        {
            InitializeComponent();

            this.images = new ImageLoader(100, 100, this.Width).LoadImages();
            this.Paint += new PaintEventHandler(ImageForm_Paint);
            this.FormClosing += new FormClosingEventHandler(ImageForm_FormClosing);
        }

        public ImageForm(IHandDataSource handDataSource)
            : this()
        {
            this.handDataSource = handDataSource;
            this.handDataSource.NewDataAvailable += new NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            this.handLayer = new HandLayer(this.handDataSource);
            this.handLayer.ShowConvexHull = false;
        }

        private void ResetHands()
        {
            this.handTracks.Clear();
            this.Invalidate();
        }

        private void UpdateHandTrackData(HandCollection handData)
        {
            foreach (var newHand in handData.Hands.Where(h => !this.handTracks.Any(t => t.Id == h.Id)))
            {
                this.handTracks.Add(new HandTracker(newHand));
            }
            foreach (var handTrack in this.handTracks.ToList())
            {
                var newHand = handData.Hands.Where(h => h.Id == handTrack.Id).FirstOrDefault();
                if (newHand == null)
                {
                    this.handTracks.Remove(handTrack);
                }
                else
                {
                    if (!newHand.HasPalmPoint)
                    {
                        continue;
                    }
                    handTrack.SetHandData(newHand);
                    var hoveredImage = this.images.Where(i => this.ImageContains(i, newHand.PalmPoint.Value)).LastOrDefault();
                    if (hoveredImage != null)
                    {
                        MoveImageToFront(hoveredImage);
                        handTrack.HandleTranslation(hoveredImage, this.handLayer.ZoomFactor);
                    }
                }
            }
        }                             
            
        private void MoveImageToFront(InteractiveImage image)
        {
            this.images.Remove(image);
            this.images.Add(image);
        }

        private bool ImageContains(InteractiveImage image, Point center)
        {
            return image.Area.Contains(MapPoint(center, this.handDataSource.Size));
        } 

        private void UnhoverImages()
        {
            foreach (var localImage in this.images)
            {
                localImage.Hovered = false;
            }
        }

        void handDataSource_NewDataAvailable(HandCollection handData)
        {
            if (handData.IsEmpty)
            {
                this.ResetHands();
                return;
            }

            this.UnhoverImages();
            this.UpdateHandTrackData(handData);

            var handsOverImages = this.handTracks.Where(h => h.IsOverImage);
            foreach (var handTrack in handsOverImages)
            {
                this.HandleTwoHandedActions(handTrack);
            }

            this.Invalidate();
        }

        private void HandleTwoHandedActions(HandTracker handTrack)
        {
            var otherHand = this.handTracks.Where(h => h != handTrack && h.HoveredImage == handTrack.HoveredImage).FirstOrDefault();
            if (otherHand == null)
            {
                handTrack.ResizeSingleHand();
            }
            else
            {
                handTrack.ResizeTwoHands(otherHand);
            }
        }

        private Point MapPoint(Point point, IntSize originalSize)
        {
            return new Point(point.X / originalSize.Width * this.ClientSize.Width, point.Y / originalSize.Height * this.ClientSize.Height, 0);
        }

        private System.Drawing.Point MapToScreen(Point point, CCT.NUI.Core.Size originalSize)
        {
            int border = 50;
            return new System.Drawing.Point(-border + (int)(point.X / originalSize.Width * (Screen.PrimaryScreen.Bounds.Width + 2 * border)), - border + (int)(point.Y / originalSize.Height * (Screen.PrimaryScreen.Bounds.Height + 2 * border)));
        }

        private void ResetImages()
        {
            
            foreach (var image in this.images.ToList())
            {
                image.Reset();
            }

            this.Invalidate();

        }

        private void AddImage()
        {
            FileInfo newestFile = null;

            foreach (string fileName in Directory.GetFiles(@"Y:\\", "*.jpg"))
            {

                FileInfo file = new FileInfo(fileName);

                if (newestFile == null || file.CreationTime > newestFile.CreationTime)

                    newestFile = file;

            }

            var newImage = new InteractiveImage((System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(newestFile.FullName), 100, 100);
            this.images.Add(newImage);
        }


        void ImageForm_Paint(object sender, PaintEventArgs e)
        {
            // Check to see if a new image has been uploaded every 50 times you paint the ImageForm
            if (reset_counter == 50)
            {
                if (Directory.EnumerateFiles(@"Y:\\", "*.jpg").Count() > this.images.Count()) // test if a new image is found
                {
                    this.AddImage(); // if it is reload the images, Adds new Photos!!!
                }

                reset_counter = 0;
            }

            reset_counter++;

            foreach (var image in this.images.ToList())
            {
                image.Draw(e.Graphics);
            }

            this.handLayer.SetTargetSize(this.Size);
            this.handLayer.SetZoomHandFactor(1 / this.handLayer.ZoomFactor);
            this.handLayer.Paint(e.Graphics);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ResetImages();
        }

        public System.Drawing.Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        private System.Drawing.Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window 
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size 
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to 
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to, 
            // using GetDeviceCaps to get the width/height 
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object 
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over 
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection 
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it 
            System.Drawing.Image img = System.Drawing.Image.FromHbitmap(hBitmap);
            // free up the Bitmap object 
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter 
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
              int nWidth, int nHeight, IntPtr hObjectSource,
              int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
              int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        } 

        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
        }

        public System.Drawing.Image CaptureActiveWindow()
        {
            return CaptureWindow(User32.GetForegroundWindow());
        } 

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Drawing.Image img = CaptureActiveWindow();
            
            img.Save(@thefilename, System.Drawing.Imaging.ImageFormat.Jpeg); 
            //markus' tweet code
            Random random = new Random();
            int number = random.Next(0, 100);
            //string base64String;
            byte[] imageBytes;
           /* using (System.Drawing.Bitmap bm = new System.Drawing.Bitmap(thefilename))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    base64String = Convert.ToBase64String(ms.ToArray());
                }
            }*/

            MemoryStream ms = new MemoryStream();

            using (System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(thefilename))
            {
                using (System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap))
                {
                    newBitmap.SetResolution(300, 300); //que?
                    newBitmap.Save(@thefilename_res, System.Drawing.Imaging.ImageFormat.Jpeg);

                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    imageBytes = ms.ToArray();
                }
            }

            

            TwitPic tw = new TwitPic();
            Debug.WriteLine("image/jpeg " + "image_ " + number + " " + thefilename_res + " " + "f1df26cd49afe58d92fff17cdd1c94bf " + "494611009-ybbVnZ9ThmVUhN65QvH0x2l48BcXUtF0pNG8AUq4 " + "YZ2eGUbGnYz1ratWweQD1fpK1JuxAUtJ4nIZBA1Y " + "cflG9inzlNltp2Znw5zEWA " + "CDS4bQi9NRdRRBGR4Am1skJNRonHbGrsBFwUmpk ");
            string upload_script = tw.UploadPhoto(imageBytes, "image/jpeg", "image_" + number, thefilename_res, "f1df26cd49afe58d92fff17cdd1c94bf", "494611009-ybbVnZ9ThmVUhN65QvH0x2l48BcXUtF0pNG8AUq4", "YZ2eGUbGnYz1ratWweQD1fpK1JuxAUtJ4nIZBA1Y", "cflG9inzlNltp2Znw5zEWA", "CDS4bQi9NRdRRBGR4Am1skJNRonHbGrsBFwUmpk").ToString();
            Debug.WriteLine("Result of upload_script =" + upload_script);



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

        void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.handDataSource.Stop();
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
