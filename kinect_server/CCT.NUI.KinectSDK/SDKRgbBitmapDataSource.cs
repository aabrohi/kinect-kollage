using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Drawing;
using System.Drawing.Imaging;

namespace CCT.NUI.KinectSDK
{
    public class SDKRgbBitmapDataSource : SDKBitmapDataSource
    {
        public SDKRgbBitmapDataSource(INuiRuntime nuiRuntime)
            : base(nuiRuntime)
        { }

        public override int Width
        {
            get { return this.NuiRuntime.VideoStreamWidth; }
        }

        public override int Height
        {
            get { return this.NuiRuntime.VideoStreamHeight; }
        }

        protected override void  InnerStart()
        {
            this.NuiRuntime.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_VideoFrameReady);
        }

        protected override void InnerStop()
        {
            this.NuiRuntime.VideoFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_VideoFrameReady);
        }

        protected void nuiRuntime_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.ProcessFrame(e.ImageFrame);
        }

        protected unsafe void ProcessFrame(ImageFrame frame)
        {
            var image = frame.Image;
            BitmapData bitmapData = this.CurrentValue.LockBits(new System.Drawing.Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte* pDest = (byte*)bitmapData.Scan0.ToPointer();
            int pointer = 0;

            var maxIndex = this.Width * this.Height;
            for (int index = 0; index < maxIndex; index++)
            {
                pDest[0] = image.Bits[pointer];
                pDest[1] = image.Bits[pointer + 1];
                pDest[2] = image.Bits[pointer + 2];
                pDest += 3;
                pointer += image.BytesPerPixel;
            }
            this.CurrentValue.UnlockBits(bitmapData);
            this.OnNewDataAvailable();
        }
    }
}
