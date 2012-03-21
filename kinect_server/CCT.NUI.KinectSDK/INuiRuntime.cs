using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace CCT.NUI.KinectSDK
{
    public interface INuiRuntime
    {
        int DepthStreamWidth { get; }

        int DepthStreamHeight { get; }

        int VideoStreamWidth { get; }

        int VideoStreamHeight { get; }
        
        event EventHandler<ImageFrameReadyEventArgs> VideoFrameReady;

        event EventHandler<ImageFrameReadyEventArgs> DepthFrameReady;
    }
}
