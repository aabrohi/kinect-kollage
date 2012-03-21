using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Video;
using CCT.NUI.Visual;
using CCT.NUI.HandTracking;
using CCT.NUI.WPFSamples.PinCode;

namespace CCT.NUI.WPFSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenNIDataSourceFactory factory;
        private IHandDataSource handDataSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.factory = new OpenNIDataSourceFactory("config.xml");
            this.handDataSource = new HandDataSource(this.factory.CreateShapeDataSource(this.factory.CreateClusterDataSource(new Core.Clustering.ClusterDataSourceSettings { MaximumDepthThreshold = 900 }), new Core.Shape.ShapeDataSourceSettings()));
            this.factory.CreateRGBImageDataSource().Start();

            var depthImageSource = this.factory.CreateDepthImageDataSource();
            depthImageSource.NewDataAvailable += new NewDataHandler<ImageSource>(MainWindow_NewDataAvailable);
            depthImageSource.Start();
            handDataSource.Start();
        }

        void MainWindow_NewDataAvailable(ImageSource data)
        {
            this.videoControl.Dispatcher.Invoke(new Action(() =>
            {
                this.videoControl.ShowImageSource(data);
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new Action(() =>
            {
                this.handDataSource.Stop();
                this.factory.DisposeAll();
            }).BeginInvoke(null, null);
        }

        private void buttonTouch_Click(object sender, RoutedEventArgs e)
        {
            new TouchWindow(this.handDataSource).Show();
        }

        private void buttonManipulation_Click(object sender, RoutedEventArgs e)
        {
            new ManipulationWindow(this.handDataSource).Show();
        }

        private void nuiLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void blogLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void checkHandLayer_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleLayers();
        }

        private void checkClusterLayer_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleLayers();
        }

        private void ToggleLayers()
        {
            var layers = new List<IWpfLayer>();
            if (this.checkHandLayer.IsChecked.GetValueOrDefault())
            {
                layers.Add(new WpfHandLayer(this.handDataSource));
            }
            if (this.checkClusterLayer.IsChecked.GetValueOrDefault())
            {
                layers.Add(new WpfClusterLayer(this.factory.CreateClusterDataSource()));
            }
            this.videoControl.Layers = layers;
        }

        private void buttonHandInterface_Click(object sender, RoutedEventArgs e)
        {
            this.factory.SetAlternativeViewpointCapability();
            new HandInterfaceWindow(this.handDataSource, this.factory.CreateRGBImageDataSource()).Show();
        }
    }
}
