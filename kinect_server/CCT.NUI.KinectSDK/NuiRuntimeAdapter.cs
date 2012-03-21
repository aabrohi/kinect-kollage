using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace CCT.NUI.KinectSDK
{
    public class NuiRuntimeAdapter : INuiRuntime, IDisposable
    {
        private Runtime runtime;

        public NuiRuntimeAdapter(Runtime runtime)
        {
            this.runtime = runtime;
            this.runtime.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_VideoFrameReady);
            this.runtime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);
        }

        public void Dispose()
        {
            this.runtime.VideoFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(runtime_VideoFrameReady);
            this.runtime.DepthFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);
        }

        public int DepthStreamWidth
        {
            get { return this.runtime.DepthStream.Width; }
        }

        public int DepthStreamHeight
        {
            get { return this.runtime.DepthStream.Height; }
        }

        public int VideoStreamWidth
        {
            get { return this.runtime.VideoStream.Width; }
        }

        public int VideoStreamHeight
        {
            get { return this.runtime.VideoStream.Height; }
        }

        public event EventHandler<ImageFrameReadyEventArgs> DepthFrameReady;

        void runtime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            if (this.DepthFrameReady != null)
            {
                this.DepthFrameReady(this, e);
            }
        }

        public event EventHandler<ImageFrameReadyEventArgs> VideoFrameReady;

        void runtime_VideoFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            if (this.VideoFrameReady != null)
            {
                this.VideoFrameReady(this, e);
            }
        }
    }
}
