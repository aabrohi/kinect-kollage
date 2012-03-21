using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.KinectSDK
{
    public class SDKClusterDataSource : RuntimeDataSource<ClusterCollection>, IClusterDataSource
    {
        private IClusterFactory clusterFactory;
        private IDepthPointFilter<ImageFrame> filter;

        public SDKClusterDataSource(INuiRuntime nuiRuntime, IClusterFactory clusterFactory, IDepthPointFilter<ImageFrame> filter)
            : base(nuiRuntime)
        {
            this.CurrentValue = new ClusterCollection();
            this.clusterFactory = clusterFactory;
            this.filter = filter;
        }

        public override int Width
        {
            get { return this.NuiRuntime.DepthStreamWidth; }
        }

        public override int Height
        {
            get { return this.NuiRuntime.DepthStreamHeight; }
        }

        protected ClusterCollection Process(ImageFrame image)
        {
            return this.clusterFactory.Create(this.FindPointsWithinDepthRange(image));
        }

        protected virtual unsafe IList<Point> FindPointsWithinDepthRange(ImageFrame image)
        {
            return this.filter.Filter(image);
        }

        protected override void InnerStart()
        {
            this.NuiRuntime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);
        }

        protected override void InnerStop()
        {
            this.NuiRuntime.DepthFrameReady -= new EventHandler<ImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);
        }

        void nuiRuntime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            this.CurrentValue = this.Process(e.ImageFrame);
            this.OnNewDataAvailable();
        }
    }
}
