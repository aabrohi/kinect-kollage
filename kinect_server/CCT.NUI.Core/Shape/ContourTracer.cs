using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    internal class ContourTracer
    {
        private DepthMap contourMap;
        private int maximumRetraceSteps;

        private IList<Point> contourPoints;

        public ContourTracer(int maximumRetraceSteps)
        {
            this.maximumRetraceSteps = maximumRetraceSteps;
        }

        public IList<Point> GetContourPoints(DepthMap contourMap)
        {
            this.contourMap = contourMap;
            this.contourPoints = new List<Point>();
            this.Process();
            return this.contourPoints;
        }

        private int Width { get { return this.contourMap.Width; } }

        private int Height { get { return this.contourMap.Height; } }

        private int ResultCount { get { return this.contourPoints.Count; } }

        private Point FindFirstPoint()
        {
            int width = this.Width;
            int height = this.Width;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (this.IsContourPoint(x, y))
                    {
                        return new Point(x, y, this.contourMap[x, y]);
                    }
                }
            }
            throw new ArgumentException("Contour has no points");
        }

        private bool IsContourPoint(int x, int y)
        {
            return this.contourMap.IsSet(x, y);
        }

        private bool IsAllowed(int x, int y)
        {
            for (int index = Math.Max(0, this.ResultCount - this.maximumRetraceSteps); index < this.ResultCount; index++) //TODO
            {
                if (this.contourPoints[index].X == x && this.contourPoints[index].Y == y)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsSetAndAllowed(int x, int y)
        {
            return this.contourMap.IsSet(x, y) && this.IsAllowed(x, y);
        }

        private Point? GetNextPoint(Point p)
        {
            foreach (var rotationPoint in PointRotation.Points)
            {
                var x = (int)(p.X + rotationPoint.X);
                var y = (int)(p.Y + rotationPoint.Y);
                if (IsSetAndAllowed(x, y))
                {
                    return new Point(x, y, p.Z);
                }
            }
            return null;
        }

        private void Process() 
        {
            var firstPoint = this.FindFirstPoint();
            var currentPoint = firstPoint;
            this.contourPoints.Add(currentPoint);
            int retraceCount = 0;

            //TODO: Find better stop condition
            while (retraceCount < maximumRetraceSteps && this.contourPoints.Count <= 1500 && !(this.contourPoints.Count > 10 && Point.Distance(currentPoint, firstPoint) < 2))
            {
                var nextPoint = this.GetNextPoint(currentPoint);
                if (nextPoint != null)
                {
                    retraceCount = 0;
                    this.contourPoints.Add(nextPoint.Value);
                    currentPoint = nextPoint.Value;
                }
                else
                {
                    currentPoint = this.contourPoints[Math.Max(0, this.contourPoints.Count - retraceCount - 1)];
                    retraceCount++;
                }
            }
            this.contourPoints.Add(firstPoint);
        }
    }

    class PointRotation
    {
        private static IList<Point> points;

        static PointRotation()
        {
            points = new List<Point>();
            points.Add(new Point(1, 0, 0));
            points.Add(new Point(1, 1, 0));
            points.Add(new Point(0, 1, 0));
            points.Add(new Point(-1, 1, 0));
            points.Add(new Point(-1, 0, 0));
            points.Add(new Point(-1, -1, 0));
            points.Add(new Point(0, -1, 0));
            points.Add(new Point(1, -1, 0));
        }

        public static IEnumerable<Point> Points
        {
            get { return points; }
        }
    }
}
