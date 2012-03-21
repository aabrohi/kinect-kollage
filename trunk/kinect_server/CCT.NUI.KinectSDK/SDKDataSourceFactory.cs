using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core.Video;

namespace CCT.NUI.KinectSDK
{
    public class SDKDataSourceFactory : IDataSourceFactory
    {
        private Runtime nuiRuntime;

        public SDKDataSourceFactory()
        {
            this.nuiRuntime = Runtime.Kinects[0];
            this.Adapter = new NuiRuntimeAdapter(this.nuiRuntime);

            try
            {
                this.nuiRuntime.Initialize(RuntimeOptions.UseColor | RuntimeOptions.UseDepth);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            try
            {
                nuiRuntime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
                nuiRuntime.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution640x480, ImageType.Depth);
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private NuiRuntimeAdapter Adapter { get; set; }

        public IImageDataSource CreateRGBImageDataSource()
        {
            return new SDKRgbImageDataSource(this.Adapter);
        }

        public IImageDataSource CreateDepthImageDataSource()
        {
            return new SDKDepthImageDataSource(this.Adapter);
        }

        public IBitmapDataSource CreateRGBBitmapDataSource()
        {
            return new SDKRgbBitmapDataSource(this.Adapter);
        }

        public IBitmapDataSource CreateDepthBitmapDataSource()
        {
            return new SDKDepthBitmapDataSource(this.Adapter);
        }

        public IClusterDataSource CreateClusterDataSource(ClusterDataSourceSettings clusterDataSourceSettings)
        {
            var size = new IntSize(this.Adapter.DepthStreamWidth, this.Adapter.DepthStreamHeight);
            var clusterFactory = new KMeansClusterFactory(clusterDataSourceSettings, size);
            var filter = new ImageFrameDepthPointFilter(size, clusterDataSourceSettings.MinimumDepthThreshold, clusterDataSourceSettings.MaximumDepthThreshold, clusterDataSourceSettings.LowerBorder);
            return new SDKClusterDataSource(this.Adapter, clusterFactory, filter);
        }

        public IClusterDataSource CreateClusterDataSource()
        {
            var settings = new ClusterDataSourceSettings();
            settings.MaximumDepthThreshold = 950;
            return this.CreateClusterDataSource(settings);
        }

        public IShapeDataSource CreateShapeDataSource()
        {
            return new ClusterShapeDataSource(this.CreateClusterDataSource());
        }

        public IShapeDataSource CreateShapeDataSource(IClusterDataSource clusterdataSource)
        {
            return new ClusterShapeDataSource(clusterdataSource, new ShapeDataSourceSettings());
        }

        public IShapeDataSource CreateShapeDataSource(IClusterDataSource clusterdataSource, ShapeDataSourceSettings shapeDataSourceSettings)
        {
            return new ClusterShapeDataSource(clusterdataSource, shapeDataSourceSettings);
        }

        public IShapeDataSource CreateShapeDataSource(ClusterDataSourceSettings clusterDataSourceSettings, ShapeDataSourceSettings shapeDataSourceSettings)
        {
            return new ClusterShapeDataSource(this.CreateClusterDataSource(clusterDataSourceSettings), shapeDataSourceSettings);
        }

        public Runtime Runtime
        {
            get { return this.nuiRuntime; }
        }

        public void DisposeAll()
        {
            this.Adapter.Dispose();
            this.nuiRuntime.Uninitialize();
        }
    }
}
