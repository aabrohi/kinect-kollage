using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.NUI.Core.Shape
{
    internal class ContourScanner
    {
        private DepthMap inputMap;
        private DepthMap result;

        public ContourScanner()
        { }

        public DepthMap Scan(DepthMap inputMap)
        {
            this.inputMap = inputMap;
            this.result = new DepthMap(inputMap.Width + 1, inputMap.Height + 1);
            Parallel.Invoke(this.ScanHorizontal, this.ScanVertical);
            return this.result;
        }

        private void ScanHorizontal()
        {
            var width = this.result.Width; //making these local increases speed
            var height = this.result.Height;
            for (int y = 0; y < height; y++)
            {
                var lastValue = 0;
                for (int x = 0; x < width; x++)
                {
                    ProcessValue(x, y, ref lastValue);
                }
            }
        }

        private void ScanVertical()
        {
            var width = this.result.Width; //making these local increases speed
            var height = this.result.Height;
            for (int x = 0; x < width; x++)
            {
                var lastValue = 0;
                for (int y = 0; y < height; y++)
                {
                    ProcessValue(x, y, ref lastValue);
                }
            }
        }

        private void ProcessValue(int x, int y, ref int lastValue)
        {
            var currentValue = this.inputMap[x, y];
            bool hasChanged;
            if (currentValue == 0)
            {
                hasChanged = lastValue > 0;
            }
            else
            {
                hasChanged = lastValue == 0;
            }
            //var hasChanged = lastValue != currentValue;

            if (hasChanged)
            {
                result[x, y] = Math.Max(currentValue, lastValue);
                lastValue = currentValue;
            }
        }
    }
}
