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
    public class ImageFrameDepthPointFilterTests
    {
        private IntSize size;
        private ImageFrameDepthPointFilter filter;

        [TestInitialize]
        public void Setup()
        {
            this.size = new IntSize(20, 10);
            this.filter = new ImageFrameDepthPointFilter(this.size, 500, 700, 0);
        }

        [TestMethod]
        public void Filters_ImageFrame_Correctly()
        {
            var result = this.filter.Filter(CreateDepthDataFrame());

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(new Point(2, 2, 700)));
            Assert.IsTrue(result.Contains(new Point(15, 8, 700)));
        }

        private ImageFrame CreateDepthDataFrame()
        {
            var frame = new ImageFrame();
            var data = new byte[this.size.Width * this.size.Height * 2];

            int index = 0;
            for (int y = 0; y < this.size.Height; y++)
            {
                for (int x = 0; x < this.size.Width; x++)
                {
                    index = (y * this.size.Width + this.size.Width -x -1) * 2; //Image is mirrored
                    int depthValue = 0;
                    if ((x == 2 && y == 2) || (x == 15 && y == 8))
                    {
                        depthValue = 700;
                    }

                    data[index] = (byte)(depthValue % 256);
                    data[index + 1] = (byte)(depthValue / 256);
                }
            }
            frame.Image.Bits = data;
            return frame;
        }
    }
}
