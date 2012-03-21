using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.Core;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.Tests.KinectSDK
{
    [TestClass]
    public class SDKRgbImageDataSourceTests
    {
        private SDKRgbBitmapDataSource dataSource;
        private RuntimeStub runtimeStub;
        private bool newDataCalled;

        [TestInitialize]
        public void Setup()
        {
            this.runtimeStub = new RuntimeStub();
            this.dataSource = new SDKRgbBitmapDataSource(runtimeStub);
            this.newDataCalled = false;
            this.dataSource.NewDataAvailable += new NewDataHandler<System.Drawing.Bitmap>(dataSource_NewDataAvailable);
        }

        void dataSource_NewDataAvailable(System.Drawing.Bitmap data)
        {
            this.newDataCalled = true;
        }

        [TestCleanup]
        public void Teardown()
        {
            this.dataSource.NewDataAvailable -= new NewDataHandler<System.Drawing.Bitmap>(dataSource_NewDataAvailable);
            this.dataSource.Dispose();
        }

        [TestMethod]
        public void Test_IsRunning()
        {
            Assert.IsFalse(this.dataSource.IsRunning);
            this.dataSource.Start();
            Assert.IsTrue(this.dataSource.IsRunning);
            this.dataSource.Stop();
            Assert.IsFalse(this.dataSource.IsRunning);
        }

        [TestMethod]
        public void Test_Size()
        {
            Assert.AreEqual(this.runtimeStub.VideoStreamWidth, this.dataSource.Size.Width);
            Assert.AreEqual(this.runtimeStub.VideoStreamHeight, this.dataSource.Size.Height);
        }

        [TestMethod]
        public void Can_Process_ImageData()
        {
            dataSource.Start();
            var frame = new ImageFrame();
            var data = new byte[this.runtimeStub.VideoStreamWidth * this.runtimeStub.VideoStreamHeight * 3];

            for (int index = 0; index < data.Length; index++)
            {
                data[index] = 123;
            }
            frame.Image.Bits = data;

            this.runtimeStub.InvokeVideoFrameReady(frame);

            Assert.IsTrue(this.newDataCalled);
            this.dataSource.Stop();
        }
    }
}
