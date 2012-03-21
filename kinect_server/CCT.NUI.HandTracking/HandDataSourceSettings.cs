using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking
{
    public class HandDataSourceSettings
    {
        public HandDataSourceSettings()
        {
            SetToDefault(this);
        }

        public float MinimumDistanceBetweenFingerPoints { get; set; }
        public float MinimumDistanceIntersectionPoints { get; set; }
        public float MinimumDistanceFingerPointToIntersectionLine { get; set; }
        public float MaximumDistanceBetweenIntersectionPoints { get; set; }

        public bool DetectFingers { get; set; }

        public bool DetectCenterOfPalm { get; set; }

        public int PalmAccuracySearchRadius { get; set; }

        public float PalmContourReduction { get; set; }

        public bool DetectFingerDirection { get; set; }

        public static void SetToDefault(HandDataSourceSettings settings)
        {
            settings.MinimumDistanceBetweenFingerPoints = 25;
            settings.MinimumDistanceIntersectionPoints = 25;
            settings.MinimumDistanceFingerPointToIntersectionLine = 22;
            settings.MaximumDistanceBetweenIntersectionPoints = 27;

            settings.DetectFingers = true;
            settings.DetectCenterOfPalm = true;
            settings.DetectFingerDirection = true;

            settings.PalmAccuracySearchRadius = 4;
            settings.PalmContourReduction = 8;
        }

        public static void SetToFast(HandDataSourceSettings settings)
        {
            SetToDefault(settings);
            settings.PalmAccuracySearchRadius = 2;
            settings.PalmContourReduction = 10;
        }

        public static void SetToAccurate(HandDataSourceSettings settings)
        {
            SetToDefault(settings);
            settings.PalmAccuracySearchRadius = 6;
            settings.PalmContourReduction = 3;
        }        
    }
}
