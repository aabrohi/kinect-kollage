﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CCT.NUI.Visual
{
    public class WpfBrushSwitcher
    {
        private static IList<Brush> brushes = new List<Brush>();
        private int currentBrush = 0;

        static WpfBrushSwitcher()
        {
            brushes.Add(Brushes.Aquamarine);
            brushes.Add(Brushes.Violet);
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.Red);
            brushes.Add(Brushes.Green);
            brushes.Add(Brushes.Yellow);
            brushes.Add(Brushes.Violet);
            brushes.Add(Brushes.Orange);
            brushes.Add(Brushes.Brown);
        }

        public Brush GetNext()
        {
            var result = brushes[currentBrush++];
            if (currentBrush>=brushes.Count)
            {
                currentBrush = 0;
            }
            return result;
        }
    }
}
