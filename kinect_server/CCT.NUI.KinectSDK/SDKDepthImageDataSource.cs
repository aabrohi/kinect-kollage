using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Windows.Media;

namespace CCT.NUI.KinectSDK
{
    public class SDKDepthImageDataSource : SDKImageDataSource
    {
        public SDKDepthImageDataSource(INuiRuntime nuiRuntime)
            : base(nuiRuntime)
        { }

        public override int Width
        {
            get { return this.NuiRuntime.DepthStreamWidth; }
        }

        public override int Height
        {
            get { return this.NuiRuntime.DepthStreamHeight; }
        }

        protected override void InnerStart()
        {
            this.NuiRuntime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(NuiRuntime_DepthFrameReady);
        }

        protected override void InnerStop()
        {
            this.NuiRuntime.DepthFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(NuiRuntime_DepthFrameReady);
        }

        void NuiRuntime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            var image = e.ImageFrame.Image;
            this.CurrentValue = System.Windows.Media.Imaging.BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Bgr32, null, image.Bits, image.Width * 4);
        }
    }
}
