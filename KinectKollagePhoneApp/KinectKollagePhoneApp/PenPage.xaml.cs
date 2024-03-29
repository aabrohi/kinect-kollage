﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework; //////////////
using Microsoft.Devices.Sensors; ////////////

namespace KinectKollagePhoneApp
{

    public partial class PenPage : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        double thresh = .5;
        bool undone = false;

        public int PenInst;
        public int StickInst;
        public int TextInst;

        private System.Windows.Point currentPoint;
        private System.Windows.Point oldPoint;
        SolidColorBrush colorBrush;
        int brushSize;
        string color = "Transparent";
        string size = "10";
        //WriteableBitmap writeableBitmapImg;

        public PenPage()
        {
            InitializeComponent();
            if (accelerometer == null)
            {
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
                accelerometer.Start();
            }
            BitmapImage bi = new BitmapImage();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                /*var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                img.Source = imageAsBitmap;
                 */
                //img.Source = imageAsBitmap;

                if (store.FileExists("tempJPEG2"))
                {
                    using (IsolatedStorageFileStream fileStream = store.OpenFile("tempJPEG2", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bi.SetSource(fileStream);
                        img.Source = bi;
                    }
                }
                else
                {
                    var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                    img.Source = imageAsBitmap;
                }


            }
            //writeableBitmapImg.Invalidate();
            // Add items to color listbox
            this.listBox1.Items.Add("Black");
            this.listBox1.Items.Add("White");
            this.listBox1.Items.Add("Blue");
            this.listBox1.Items.Add("Green");
            this.listBox1.Items.Add("Red");
            this.listBox1.Items.Add("Yellow");
            this.listBox1.Items.Add("Purple");


            // Add items to size listbox
            this.listBox2.Items.Add("1");
            this.listBox2.Items.Add("2");
            this.listBox2.Items.Add("3");
            this.listBox2.Items.Add("4");
            this.listBox2.Items.Add("5");
            this.listBox2.Items.Add("6");
            this.listBox2.Items.Add("7");
            this.listBox2.Items.Add("8");
            this.listBox2.Items.Add("9");
            this.listBox2.Items.Add("10");
            this.listBox2.Items.Add("11");
            this.listBox2.Items.Add("12");
            this.listBox2.Items.Add("13");
            this.listBox2.Items.Add("14");

            //this.imageCanvas.MouseMove += new MouseEventHandler(img_OnMouseMove);
            //this.imageCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(img_OnMouseLeftButtonDown);

            colorBrush = new SolidColorBrush(Colors.Purple);
            brushSize = 5;

        }
        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => undo(e.SensorReading));
        }

        private void undo(AccelerometerReading acceleromterReading)
        {
            //MessageBox.Show("I shook the phone?");
            Vector3 acceleration = acceleromterReading.Acceleration;
            if ((acceleration.X > thresh || acceleration.Y > thresh || acceleration.Z > thresh) && !undone)
            {
                BitmapImage bi = new BitmapImage();
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream fileStream = store.OpenFile("undoJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bi.SetSource(fileStream);
                        img.Source = bi;
                        store.CopyFile("undoJPEG", "tempJPEG2", true);
                        fileStream.Close();
                    }
                }
                undone = true;
                //MessageBox.Show("I shook the phone?");
            }
        }
        /*void img_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentPoint = e.GetPosition(imageCanvas);
            oldPoint = currentPoint;
        }

        //void img_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    writeableBitmapImg.Invalidate();
        //}

        void img_OnMouseMove(object Sender, MouseEventArgs e)
        {
            currentPoint = e.GetPosition(imageCanvas);
            //writeableBitmapImg.DrawLine((int)currentPoint.X, (int)currentPoint.Y, (int)oldPoint.X, (int)oldPoint.Y, Colors.Purple);
            //writeableBitmapImg.Invalidate();
            Line line = new Line() { X1 = currentPoint.X, Y1 = currentPoint.Y, X2 = oldPoint.X, Y2 = oldPoint.Y };
            line.Stroke = colorBrush;
            line.StrokeThickness = brushSize;
            this.imageCanvas.Children.Add(line);
            oldPoint = currentPoint;
        }
        */
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //BinaryReader objReader = new BinaryReader(writeableBitmapImg.ToByteArray);
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG");

            WriteableBitmap wb = new WriteableBitmap(imageCanvas, null);
            wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
            myFileStream.Close();

            myFileStream = myStore.OpenFile("tempJPEG", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture("NewPicture.jpg", myFileStream);
            //MessageBox.Show("Image saved");
            myFileStream.Close();

            // Save to image.jpg
            //WriteableBitmap bmp = new WriteableBitmap(img,null);
            //bmp.SaveJpeg(local,,,0,100);

        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditPage.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var filestream = store.OpenFile("image.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var imageAsBitmap = Microsoft.Phone.PictureDecoder.DecodeJpeg(filestream);
                img.Source = imageAsBitmap;
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string data = (sender as ListBox).SelectedItem as string;
            color = data;
            if (data == "Black")
                colorBrush = new SolidColorBrush(Colors.Black);
            else if (data == "White")
                colorBrush = new SolidColorBrush(Colors.White);
            else if (data == "Blue")
                colorBrush = new SolidColorBrush(Colors.Blue);
            else if (data == "Green")
                colorBrush = new SolidColorBrush(Colors.Green);
            else if (data == "Red")
                colorBrush = new SolidColorBrush(Colors.Red);
            else if (data == "Yellow")
                colorBrush = new SolidColorBrush(Colors.Yellow);
            else if (data == "Purple")
                colorBrush = new SolidColorBrush(Colors.Purple);

        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string data = (sender as ListBox).SelectedItem as string;
            size = data;
            if (data == "1")
                brushSize = 1;
            else if (data == "2")
                brushSize = 2;
            else if (data == "3")
                brushSize = 3;
            else if (data == "4")
                brushSize = 4;
            else if (data == "5")
                brushSize = 5;
            else if (data == "6")
                brushSize = 6;
            else if (data == "7")
                brushSize = 7;
            else if (data == "8")
                brushSize = 8;
            else if (data == "9")
                brushSize = 9;
            else if (data == "10")
                brushSize = 10;
            else if (data == "11")
                brushSize = 11;
            else if (data == "12")
                brushSize = 12;
            else if (data == "13")
                brushSize = 13;
            else if (data == "14")
                brushSize = 14;

        }
        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            //MessageBox.Show(this.Orientation.ToString());
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {

            }
            else
            {
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (myStore.FileExists("tempJPEG2"))
                {
                    myStore.DeleteFile("tempJPEG2");
                }

                IsolatedStorageFileStream myFileStream = myStore.CreateFile("tempJPEG2");

                WriteableBitmap wb = new WriteableBitmap(imageCanvas, null);
                wb.SaveJpeg(myFileStream, 1000, 667, 0, 100);
                myFileStream.Close();

                /////////////////////////////// UNDO
                if (myStore.FileExists("undoJPEG"))
                {
                    myStore.DeleteFile("undoJPEG");
                }

                IsolatedStorageFileStream myUndoStream = myStore.CreateFile("undoJPEG");

                WriteableBitmap uwb = new WriteableBitmap(imageCanvas, null);
                uwb.SaveJpeg(myUndoStream, 1000, 667, 0, 100);
                myUndoStream.Close();
                //////////////////////////////// \UNDO

                string url = "/HorizPenPage.xaml?color=";
                url += color.ToString();
                //MessageBox.Show(color);
                url += "&size=";
                url += size.ToString();
                url += "&PI=";
                url += PenInst.ToString();
                url += "&SI=";
                url += StickInst.ToString();
                url += "&TI=";
                url += TextInst.ToString();
                //MessageBox.Show(size);
                NavigationService.Navigate(new Uri(url, UriKind.Relative));
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            string url = "/EditPage.xaml?PI=";
            url += PenInst.ToString();
            url += "&SI=";
            url += StickInst.ToString();
            url += "&TI=";
            url += TextInst.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
            e.Cancel = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //MessageBox.Show("navigated to shapepage");
            string temp = NavigationContext.QueryString["PI"];
            PenInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["SI"];
            StickInst = Convert.ToInt32(temp);
            temp = NavigationContext.QueryString["TI"];
            TextInst = Convert.ToInt32(temp);

            if (PenInst == 0)
            {
                MessageBox.Show("Select a pen color and thickness, then rotate the phone horizontally to draw using that brush type. "
                        + "Rotate the phone vertically to select a different brush type and/or return to the menu. Shake the phone when vertical to undo the previous edits.");
                PenInst = 1;
            }

            //MessageBox.Show(StickInst.ToString());

        }
    }
}