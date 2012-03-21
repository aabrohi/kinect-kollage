using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.Tests.KinectSDK
{
    [TestClass]
    public class SDKDepthImageDataSourceTests
    {
        private SDKDepthBitmapDataSource dataSource;
        private RuntimeStub runtimeStub;

        [TestInitialize]
        public void Setup()
        {
            this.runtimeStub = new RuntimeStub();
            this.dataSource = new SDKDepthBitmapDataSource(runtimeStub);
        }

        [TestCleanup]
        public void Teardown()
        {
            this.dataSource.Dispose();
        }

        [TestMethod]
        public void Can_Process_DepthImageData()
        {
            dataSource.Start();
            var frame = new ImageFrame();
            var data = new byte[this.runtimeStub.VideoStreamWidth * this.runtimeStub.VideoStreamHeight * 3];

            for (int index = 0; index < data.Length; index++)
            {
                data[index] = 123;
            }
            frame.Image.Bits = data;

            this.runtimeStub.InvokeDepthFrameReady(frame);
            this.dataSource.Stop();
        }
    }
}
