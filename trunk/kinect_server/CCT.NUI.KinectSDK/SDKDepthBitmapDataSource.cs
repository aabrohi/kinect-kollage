using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Drawing;
using System.Drawing.Imaging;

namespace CCT.NUI.KinectSDK
{
    public class SDKDepthBitmapDataSource : SDKBitmapDataSource
    {
        public SDKDepthBitmapDataSource(INuiRuntime nuiRuntime)
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
            this.NuiRuntime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);
        }

        protected override void InnerStop()
        {
            this.NuiRuntime.DepthFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);
        }

        unsafe void nuiRuntime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            var image = e.ImageFrame.Image;
            BitmapData bitmapData = this.CurrentValue.LockBits(new System.Drawing.Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int pointer = 0;
            for (int y = 0; y < this.Height; y++)
            {
                byte* pDest = (byte*)bitmapData.Scan0.ToPointer() + y * bitmapData.Stride + bitmapData.Stride - 3;
                for (int x = 0; x < this.Width; x++, pointer += 2, pDest -= 3)
                {
                    int realDepth = image.Bits[pointer] | (image.Bits[pointer + 1] << 8);
                    byte intensity = (byte)(255 - (255 * realDepth / 0x0fff));
                    pDest[0] = intensity;
                    pDest[1] = intensity;
                    pDest[2] = intensity;
                }
            }
            this.CurrentValue.UnlockBits(bitmapData);
            this.OnNewDataAvailable();
        }
    }
}
