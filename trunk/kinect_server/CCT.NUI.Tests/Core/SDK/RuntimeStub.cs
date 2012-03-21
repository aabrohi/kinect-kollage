using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.Tests.KinectSDK
{
    public class RuntimeStub : INuiRuntime
    {   
        public int DepthStreamWidth
        {
            get { return 20; }
        }

        public int DepthStreamHeight
        {
            get { return 10; }
        }

        public int VideoStreamWidth
        {
            get { return 20; }
        }

        public int VideoStreamHeight
        {
            get { return 10; }
        }

        public event EventHandler<ImageFrameReadyEventArgs> VideoFrameReady;

        public event EventHandler<ImageFrameReadyEventArgs> DepthFrameReady;

        public void InvokeVideoFrameReady(ImageFrame frame)
        {
            if (this.VideoFrameReady != null) 
            {
                this.VideoFrameReady(this, new ImageFrameReadyEventArgs { ImageFrame = frame });
            }
        }

        public void InvokeDepthFrameReady(ImageFrame frame)
        {
            if (this.DepthFrameReady != null)
            {
                this.DepthFrameReady(this, new ImageFrameReadyEventArgs { ImageFrame = frame });
            }
        }
    }
}
