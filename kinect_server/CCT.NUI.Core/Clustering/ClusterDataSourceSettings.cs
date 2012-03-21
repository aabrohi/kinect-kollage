using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CCT.NUI.Core.Clustering
{
    public class ClusterDataSourceSettings
    {
        public ClusterDataSourceSettings()
        {
            SetToDefault(this);
        }

        public int ClusterCount { get; set; }

        public int LowerBorder { get; set; }
        public int PointModulo { get; set; }

        public int MinimumDepthThreshold { get; set; }
        public int MaximumDepthThreshold { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Range DepthRange { get { return new Range(this.MinimumDepthThreshold, this.MaximumDepthThreshold); } }

        public int MinimalPointsForClustering { get; set; }
        public int MinimalPointsForValidCluster { get; set; }

        public double MergeMinimumDistanceToCluster { get; set; }
        public double MergeMaximumClusterCenterDistances { get; set; }
        public double MergeMaximumClusterCenterDistances2D { get; set; }

        public int? MaximumClusterDepth { get; set; }

        public static void SetToDefault(ClusterDataSourceSettings settings)
        {
            settings.MinimalPointsForValidCluster = 10;
            settings.MinimalPointsForClustering = 50;
            settings.PointModulo = 5;
            settings.LowerBorder = 75;
            settings.MinimumDepthThreshold = 500;
            settings.MaximumDepthThreshold = 800;
            settings.ClusterCount = 2;

            settings.MergeMinimumDistanceToCluster = 50;
            settings.MergeMaximumClusterCenterDistances = 120;
            settings.MergeMaximumClusterCenterDistances2D = 100;

            settings.MaximumClusterDepth = 150;
        }
    }
}
