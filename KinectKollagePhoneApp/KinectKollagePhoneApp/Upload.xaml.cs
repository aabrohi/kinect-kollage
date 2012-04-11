using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Controls;
using Microsoft.Live;
using Microsoft.Live.Controls;

using System.Diagnostics;


namespace WP7_SkyDrive_Live_Connect
{
    public partial class MainPage : PhoneApplicationPage
    {
        
        private LiveConnectClient client;
        private CameraCaptureTask cameraCaptureTask;
    //    private string cameraRollFileName = "MyCameraRollPicture.jpg";
        static Random _r = new Random();
        private string cameraRollFileName = "MyCameraRollPicture.jpg";
        private string skyDriveFolderName = "IsolatedStorageFolder";
        private string skyDriveFolderID = string.Empty;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Initialize the CameraCaptureTask and assign the Completed handler in the page constructor.
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
        }

        private void btnSignin_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {
            if (e.Session != null &&
                e.Session.Status == LiveConnectSessionStatus.Connected)
            {
                client = new LiveConnectClient(e.Session);
                infoTextBlock.Text = "Signed in.";
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(btnSignin_GetCompleted);
                client.GetAsync("me", null);
            }
            else
            {
                infoTextBlock.Text = "Not signed in.";
                client = null;
            }
        }

        void btnSignin_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ContainsKey("first_name") &&
                    e.Result.ContainsKey("last_name"))
                {
                    if (e.Result["first_name"] != null &&
                        e.Result["last_name"] != null)
                    {
                        infoTextBlock.Text =
                            "Hello, " +
                            e.Result["first_name"].ToString() + " " +
                            e.Result["last_name"].ToString() + "!";
                    }
                }
                else
                {
                    infoTextBlock.Text = "Hello, signed-in user!";
                }
            }
            else
            {
                infoTextBlock.Text = "Error calling API: " +
                    e.Error.ToString();
            }
        }

        private void Camera_Click(object sender, EventArgs e)
        {
            try
            {
                cameraCaptureTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                // Catch the exception, but no handling is necessary.
            }
        }

        private void Roll_Click(object sender, EventArgs e)
        {
            PhotoChooserTask objPhotoChooser = new PhotoChooserTask();
            objPhotoChooser.Completed += new EventHandler<PhotoResult>(PhotoChooseCall);
            objPhotoChooser.Show();
        }

        void PhotoChooseCall(object sender, PhotoResult e)
        {
            switch (e.TaskResult)
            {
                case TaskResult.OK:
                    BitmapImage bmp = new BitmapImage();
                    Stream capturedImageStream = e.ChosenPhoto;
                    bmp.SetSource(capturedImageStream);
                    Uri uriImage = this.SaveImageToIS(bmp);
                    break;
                case TaskResult.Cancel:
                    MessageBox.Show("Cancelled");
                    break;
                case TaskResult.None:
                    MessageBox.Show("Nothing Entered");
                    break;
            }
        }

        // The Completed event handler. A new BitmapImage is created and
        // the source is set to the result stream from the CameraCaptureTask
        private void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                Stream capturedImageStream = e.ChosenPhoto;
                bmp.SetSource(capturedImageStream);
                Uri uriImage = this.SaveImageToIS(bmp);
            }
        }

        private Uri SaveImageToIS(BitmapImage bitmapImage)
        {
            cameraRollFileName = "Picture_" + _r.Next(100) + ".jpg";
            Uri uri = new Uri(cameraRollFileName, UriKind.Relative);
            string file = uri.ToString();
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
        //        if (store.FileExists(file))
        //            store.DeleteFile(file);
                IsolatedStorageFileStream fileStream = store.CreateFile(file);
                StreamResourceInfo sri = null;
                sri = Application.GetResourceStream(uri);
                WriteableBitmap wb = new WriteableBitmap(bitmapImage);
                Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                fileStream.Close();
            }

            return uri;
        }


        private void UploadPhoto_Click(object sender, EventArgs e)
        {

            if (client == null || client.Session == null || client.Session.Status != LiveConnectSessionStatus.Connected)
            {
                MessageBox.Show("You must sign in first.");
            }
            else
            {
                client.GetCompleted += new EventHandler<LiveOperationCompletedEventArgs>(GetFolderProperties_Completed);
                // If you put photo to folder it becomes album.
                client.GetAsync("me/skydrive/files?filter=folders,albums");
            }
        }



        private void GetFolderProperties_Completed(object sender, LiveOperationCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                Dictionary<string, object> folderData = (Dictionary<string, object>)e.Result;
                List<object> folders = (List<object>)folderData["data"];

                foreach (object item in folders)
                {
                    Dictionary<string, object> folder = (Dictionary<string, object>)item;
                    if (folder["name"].ToString() == skyDriveFolderName)
                        skyDriveFolderID = folder["id"].ToString();
                }

                if (skyDriveFolderID == string.Empty)
                {
                    Dictionary<string, object> skyDriveFolderData = new Dictionary<string, object>();
                    skyDriveFolderData.Add("name", skyDriveFolderName);
                    //You can add a folder description, but for some reason it does not work.
                    //folderData.Add("description", "Folder for storing files from my WP7 app isolated storage.");
                    client.PostCompleted += new EventHandler<LiveOperationCompletedEventArgs>(CreateFolder_Completed);
                    client.PostAsync("me/skydrive", skyDriveFolderData);
                }
                else
                    UploadFile();
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }


        private void CreateFolder_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                infoTextBlock.Text = "Folder created.";
                Dictionary<string, object> folder = (Dictionary<string, object>)e.Result;
                skyDriveFolderID = folder["id"].ToString();
                UploadFile();
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void UploadFile()
        {
            if (skyDriveFolderID != string.Empty)
            {
                string[] filePathSegments = cameraRollFileName.Split('\\');
                string fileName = filePathSegments[filePathSegments.Length - 1];
                var scopes = new List<string>(1);
                MessageBox.Show("File uploaded.");
              //  this.client.UploadCompleted
              //      += new EventHandler<LiveOperationCompletedEventArgs>(ISFile_UploadCompleted);

                IsolatedStorageFileStream fileStream = null;
                try
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        fileStream = store.OpenFile(cameraRollFileName, FileMode.Open, FileAccess.Read);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                this.client.UploadAsync(skyDriveFolderID, cameraRollFileName, true, fileStream, null);
            }
        }


        private void ISFile_UploadCompleted(object sender, LiveOperationCompletedEventArgs args)
        {
            if (args.Error == null)
            {
                Dictionary<string, object> file = (Dictionary<string, object>)args.Result;
                // For some reason SkyDrive link property is not part of result.
              //  this.infoTextBlock.Text = "File uploaded.";
                MessageBox.Show("File uploaded.");
            }
            else
            {
                MessageBox.Show("Error uploading file: " + args.Error.ToString());
              //  this.infoTextBlock.Text =
              //      "Error uploading file: " + args.Error.ToString();
              //  Debug.WriteLine(args.Error.ToString());
            }
        }

    }//Class

}//Namespace