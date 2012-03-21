using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    internal class PalmFinder
    {
        private Palm result;

        private float contourReduction;
        private int searchRadius;

        public PalmFinder(HandDataSourceSettings settings)
        {
            this.searchRadius = settings.PalmAccuracySearchRadius;
            this.contourReduction = settings.PalmContourReduction;
        }

        public Palm FindCenter(Contour contour, IList<Point> candidates)
        {
            this.result = null;
            var minimizedContour = new LineThinner(contourReduction, false).Filter(contour.Points);
            this.FindCenterFromCandidates(minimizedContour, candidates);
            this.IncreaseAccuracy(this.result.Location, minimizedContour);
            return result;
        }

        private void FindCenterFromCandidates(IList<Point> contour, IList<Point> candidates)
        {
            double[] distances = new double[candidates.Count];

            Parallel.For(0, candidates.Count, (index) =>
            {
                distances[index] = FindMaxDistance(contour, candidates[index]);
            });

            double maxDistance = this.result == null ? 0 : this.result.DistanceToContour;
            int maxIndex = -1;
            for (int index = 0; index < distances.Length; index++)
            {
                if (distances[index] > maxDistance)
                {
                    maxDistance = distances[index];
                    maxIndex = index;
                }
            }
            if (maxIndex >= 0)
            {
                this.result = new Palm(candidates[maxIndex], maxDistance);
            }
        }

        private void IncreaseAccuracy(Point center, IList<Point> contour)
        {
            var newCandidatePoints = new List<Point>();

            for (int x = -this.searchRadius; x <= this.searchRadius; x++)
            {
                for (int y = -this.searchRadius; y <= this.searchRadius; y++)
                {
                    if(x != 0 && y != 0)
                    {
                        newCandidatePoints.Add(new Point(center.X + x, center.Y + y, center.Z));
                    }
                }
            }
            this.FindCenterFromCandidates(contour, newCandidatePoints);
        }

        private double FindMaxDistance(IList<Point> contourPoints, Point candidate)
        {
            double result = double.MaxValue;
            foreach (var point in contourPoints)
            {
                result = Math.Min(Point.Distance(point.X, point.Y, candidate.X, candidate.Y), result);
            }
            return result;
        }
    }
}
