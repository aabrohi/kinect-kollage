using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.Tests.KinectSDK
{
    [TestClass]
    public class SDKClusterDataSourceTests : TestBase
    {
        private RuntimeStub runtimeStub;
        private SDKClusterDataSource clusterDataSource;
        
        private Mock<IClusterFactory> clusterFactoryMock;
        private Mock<IDepthPointFilter<ImageFrame>> filterMock;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            this.runtimeStub = new RuntimeStub();
            this.clusterFactoryMock = CreateMock<IClusterFactory>();
            this.filterMock = CreateMock<IDepthPointFilter<ImageFrame>>();

            this.clusterDataSource = new SDKClusterDataSource(this.runtimeStub, this.clusterFactoryMock.Object, this.filterMock.Object);
            this.clusterDataSource.Start();
        }

        [TestCleanup]
        public override void Teardown()
        {
            this.clusterDataSource.Stop();
            this.clusterDataSource.Dispose();
            base.Teardown();
        }

        [TestMethod]
        public void Test_Size_Properties()
        {
            Assert.AreEqual(this.runtimeStub.DepthStreamWidth, this.clusterDataSource.Width);
            Assert.AreEqual(this.runtimeStub.DepthStreamHeight, this.clusterDataSource.Height);
        }

        [TestMethod]
        public void DataSource_Filters_Then_Calls_ClusterFactory()
        {
            var imageFrame = new ImageFrame();
            var filteredPoints = new List<Point>();

            this.filterMock.Setup(m => m.Filter(imageFrame)).Returns(filteredPoints);
            this.clusterFactoryMock.Setup(m => m.Create(filteredPoints)).Returns(new ClusterCollection());

            this.runtimeStub.InvokeDepthFrameReady(imageFrame);
        }
    }
}
