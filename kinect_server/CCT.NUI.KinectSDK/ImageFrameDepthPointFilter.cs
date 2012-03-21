using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.KinectSDK
{
    public class ImageFrameDepthPointFilter : IDepthPointFilter<ImageFrame>
    {
        private IntSize size;
        private int minimumDepthThreshold;
        private int maximumDepthThreshold;
        private int lowerBorder;

        public ImageFrameDepthPointFilter(IntSize size, int minimumDepthThreshold, int maximumDepthThreshold, int lowerBorder)
        {
            this.size = size;
            this.minimumDepthThreshold = minimumDepthThreshold;
            this.maximumDepthThreshold = maximumDepthThreshold;
            this.lowerBorder = lowerBorder;
        }

        public IList<Point> Filter(ImageFrame source)
        {
            var result = new List<Point>();

            int localHeight = this.size.Height; //5ms faster when it's a local variable
            int localWidth = this.size.Width;
            int maxY = localHeight - this.lowerBorder;
            int minDepth = this.minimumDepthThreshold;
            int maxDepth = this.maximumDepthThreshold;
            var bits = source.Image.Bits;
            int pointer = 0;

            for (int y = 0; y < localHeight; y++)
            {
                for (int x = localWidth - 1; x >= 0; x--)
                {
                    int depthValue = bits[pointer] | (bits[pointer + 1] << 8);
                    if (depthValue > 0 && y < maxY && depthValue <= maxDepth && depthValue >= minDepth) //Should not be put in a seperate method for performance reasons
                    {
                        result.Add(new Point(x, y, depthValue));
                    }
                    pointer += 2;
                }
            }
            return result;
        }
    }
}
