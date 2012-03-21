using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.KinectSDK
{
    public abstract class SDKImageDataSource : RuntimeDataSource<ImageSource>, IImageDataSource
    {
        protected WriteableBitmap writeableBitmap;

        public SDKImageDataSource(INuiRuntime nuiRuntime)
            : base(nuiRuntime)
        {
            this.writeableBitmap = new WriteableBitmap(this.Width, this.Height, 96, 96, PixelFormats.Bgr24, null);
            this.CurrentValue = this.writeableBitmap;
        }
    }
}
