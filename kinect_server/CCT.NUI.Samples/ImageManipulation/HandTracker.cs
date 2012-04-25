using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using OpenNI;

namespace CCT.NUI.Samples.ImageManipulation
{
    public class HandTracker
    {
        private bool isDragging = false;
        private bool isResizing = false;

        private Point startDragPoint;
        private Point startDragPoint2;

        private int pts_gathered = 0;

        private InteractiveImage hoveredImage;

        private HandData handData;

        public HandTracker(HandData handData)
        {
            this.handData = handData;
        }

        public int Id { get { return this.handData.Id; } }

        public void SetHandData(HandData newData)
        {
            this.handData = newData;
        }
        public InteractiveImage HoveredImage 
        {
            get { return this.hoveredImage; }
        }

        public void HandleTranslation(InteractiveImage image, float zoomFactory)
        {
            this.hoveredImage = image;
            hoveredImage.Hovered = true;
            
            pts_gathered++;

            if (isResizing)
            {
                return;
            }
            var handClosed = handData.FingerCount <= 1;
            if (isDragging)
            {
                hoveredImage.Translate((handData.PalmPoint.Value.X - startDragPoint.X) * zoomFactory, (handData.PalmPoint.Value.Y - startDragPoint.Y) * zoomFactory);
            }
            if (handClosed)
            {
                //if (pts_gathered % 2 == 0)
                    startDragPoint = new Point(handData.PalmPoint.Value.X, handData.PalmPoint.Value.Y, 0);
                /*else
                {
                    startDragPoint.X += handData.PalmPoint.Value.X;
                    startDragPoint.Y += handData.PalmPoint.Value.Y;

                    startDragPoint.X = startDragPoint.X / pts_gathered % 2;
                    startDragPoint.Y = startDragPoint.Y / pts_gathered % 2;
                }*/
            }
            isDragging = handClosed;
        }

        public void ResizeSingleHand()
        {
            if (handData.FingerCount == 2)
            {
                this.HandleResize(handData.FingerPoints[0].Location, handData.FingerPoints[1].Location);
                this.isResizing = true;
            }
            else
            {
                this.isResizing = false;
            }
        }

        public void ResizeTwoHands(HandTracker otherHand)
        {
            if (!handData.HasPalmPoint || !otherHand.handData.HasPalmPoint)
            {
                return;
            }
            if (handData.FingerCount <= 1 && otherHand.handData.FingerCount <= 1)
            {
                this.HandleResize(handData.PalmPoint.Value, otherHand.handData.PalmPoint.Value);
                this.isResizing = true;
            }
            else
            {
                this.isResizing = false;
            }
        }

        private void HandleResize(Point p1, Point p2)
        {
            if (isResizing)
            {
                hoveredImage.Resize(startDragPoint, startDragPoint2, p1, p2);
            }
            startDragPoint = p1;
            startDragPoint2 = p2;
        }

        public bool IsOverImage { get { return this.hoveredImage != null; } }
    }
}
