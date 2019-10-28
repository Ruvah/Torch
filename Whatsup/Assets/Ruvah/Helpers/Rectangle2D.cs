using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.XR;

namespace Ruvah
{
    public struct Rectangle2D
    {
        public Vector2 LeftBottom;
        public Vector2 LeftTop;
        public Vector2 RightTop;
        public Vector2 RightBottom;

        public float MinimumX { get; private set; }
        public float MaximumX { get; private set; }
        public float MinimumY { get; private set; }
        public float MaximumY { get; private set; }

        
        public Rectangle2D(Vector2[] points) : this(new List<Vector2>(points))
        {

        }

        public Rectangle2D(List<Vector2> points_list)
        {
            points_list.Sort((a,b) => a.y.CompareTo(b.y));
            MinimumY = points_list[0].y;
            MaximumY = points_list[3].y;
            if (points_list[0].x < points_list[1].x)
            {
                RightBottom = points_list[1];
                LeftBottom = points_list[0];
            }
            else
            {
                RightBottom = points_list[0];
                LeftBottom = points_list[1];
            }
            
            if (points_list[2].x < points_list[3].x)
            {
                RightTop = points_list[3];
                LeftTop = points_list[2];
            }
            else
            {
                RightTop = points_list[2];
                LeftTop = points_list[3];
            }

            if (RightTop.x > RightBottom.x)
            {
                MaximumX = RightTop.x;
                MinimumX = RightBottom.x;
            }
            else
            {
                MinimumX = RightTop.x;
                MaximumX = RightBottom.x;
            }
        }

        public bool Contains(Vector2 point)
        {
            return false;
        }
    }
}
