using System;
using UnityEngine;

namespace asim.unity.utils.geometry
{
    public class GeometryUtils
    {
        /// <summary>
        /// return the max distance of p of two other points
        /// </summary>
        public static float MaxDistance(Vector2 p, Vector2 p1, Vector2 p2)
        {
            return Mathf.Abs((p.y - p1.y) * (p2.x - p1.x) - (p2.y - p1.y) * (p.x - p1.x));
        }

        //SignedVolume are volumes that can be either positive or negative, depending on the winding
        //depending on the orientation in space of the region whose volume is being measured.
        //The volume is positive if d is to the left of the plane defined by the triangle(a, b, c).
        //IMPORTANT NOTE, this might result in floating point precision issue, which causes wrong values
        public static double SignedVolume(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            //first check if any points are same this will prevent possible floating point errors when doing dot and cross product
            if (a == b || b == c || a == c) return 0;

            return Vector3.Dot(a - d, Vector3.Cross(b - d, c - d)) / 6;
        }


        





        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using cross product of 2d vector
        /// this is also known as calculating the determinant of two vector
        /// by checking the sign of cross product, will give us the direction, or which side the point is
        /// relative to the vector
        /// returns 1 if point is going to the left.
        /// returns -1 if point is going to the right.
        /// returns 0 if point is going straight/ lines on vector
        /// </summary>
        public static int Orientation(Vector2 p1, Vector2 p2, Vector2 p)
        {
            //First Vector v1 = p2-p1
            //Second Vector v2 = p-p1
            //Cross Product/det of (v1,v2) = v1.x*v2.y - v1
            //by checking the sign of cross product, will give us the direction, or which side the point is

            float val = (p2.x - p1.x) * (p.y - p1.y) -
                        (p2.y - p1.y) * (p.x - p1.x);

            if (val == 0) return 0;  // colinear
            return (int)Mathf.Sign(val); // CCW or CW
        }

        /// <summary>
        /// compute direction of points going to the left or right of vector v1->v2 using dot product
        /// this works by calculating the normal of the vector (simply by flipping the x=y,y=-x and negate one component)
        /// calculate the shortest signed distance of the vector to the point,
        /// check the sign to see if its pointing the same direction as normal vector
        /// this optimizes to computing the sign of dot product of (p-p1,normal) 
        /// </summary>
        public static int Orientation2(Vector2 p1, Vector2 p2, Vector2 p)
        {
            //p1->p = (p.x - p1.x,p.y - p1.y)
            //normal = (p2.y - p1.y,p1.x - p2.x)
            //dot = (p.x - p1.x)(p2.y - p1.y) + (p.y - p1.y)*(p1.x - p2.x)

            float val = (p.x - p1.x) * (p2.y - p1.y) - (p.y - p1.y) * (p2.x - p1.x);

            if (val == 0) return 0;  // colinear
            return (int)Mathf.Sign(-val); // CCW or CW
        }









        /// <summary>
        /// Check to see if Point is within certain radius of a target center
        /// </summary>
        public static int IsPointInRadius(Vector3 targetCenter,float radius,Vector3 point)
        {
            //use Square magnitud to save a squareroot call
            float distance = (point - targetCenter).sqrMagnitude;
            float val = radius * radius - distance;

            if (val == 0) return 0;  // on circle
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Check to see if Point is within certain radius of a target center
        /// </summary>
        public static int IsPointInEllipse(Vector2 targetCenter, Vector2 radius,float rotation, Vector2 point)
        {
            var xdiff = (point.x - targetCenter.x);
            var ydiff = (point.y - targetCenter.y);

            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);

            float val = ((cos * xdiff + sin * ydiff) * (cos * xdiff + sin * ydiff)) / (radius.x * radius.x) +
                        ((sin * xdiff - cos * ydiff) * (sin * xdiff - cos * ydiff)) / (radius.y * radius.y);

            val = 1 - val;

            if (val == 0) return 0;  // on ellipse
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Get area of Triangle using 3 points
        /// </summary>
        public static float TriangleGetArea(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return Mathf.Abs(p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f;
        }

        #region Point in Triangle http://totologic.blogspot.com/2014/01/accurate-point-in-triangle-test.html
        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// Based on "book Real Time Collision Detection" 
        /// uses barycentric coordinates method
        /// </summary>
        public static int IsPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 point)
        {
            //calculate barycentric based on p1,p2
            var denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));
            var a = ((p2.y - p3.y) * (point.x - p3.x) + (p3.x - p2.x) * (point.y - p3.y)) / denominator;
            var b = ((p3.y - p1.y) * (point.x - p3.x) + (p1.x - p3.x) * (point.y - p3.y)) / denominator;
            //var c = 1 - a - b;

            if (a < 0 || b < 0) return -1; // outside
            if (a + b > 1) return -1;// outside
            if (a * b == 0) return 0;//on Triangle;
            return 1;//intriangle
        }

        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// Uses the Same-Side or orientation method
        /// </summary>
        public static int IsPointInTriangleOrientation(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 point)
        {
            //Check Orientation of point on p1->p2 and check if its same side as p3 on p1->p2
            var o1 = Orientation(p1, p2, point) * Orientation(p1, p2, p3); // if same side, result should be +1
            //check if result is 0, means a point lines parallel to an edge on triangle,
            if (o1 == 0) return (point - p1).magnitude <= (p2 - p1).magnitude ? 0 : -1;

            //repeat
            var o2 = Orientation(p2, p3, point) * Orientation(p2, p3, p1);
            if (o2 == 0) return (point - p2).magnitude <= (p3 - p2).magnitude ? 0 : -1;

            //repeat
            var o3 = Orientation(p3, p1, point) * Orientation(p3, p1, p2);
            if (o3 == 0) return (point - p3).magnitude <= (p1 - p3).magnitude ? 0 : -1;

            if (o1 == 1 && o2 == 1 && o3 == 1) return 1;//in triangle
            return -1;// outside triangle
        }

        /// <summary>
        /// Check if Point is in triangle , on triangle, or outside of triangle
        /// This uses the easy understandable Area methord
        /// </summary>
        public static int IsPointInTriangleArea(Vector2 p1,Vector2 p2,Vector2 p3, Vector2 p)
        {
            //0. using point p, split the triangle into 3 small triangles with point p as the shared vertex

            //1. Calculate Area of main triangle
            float mainarea = TriangleGetArea(p1, p2, p3);

            //2. Calculate Area of 3 small triangle
            float t1area = TriangleGetArea(p, p2, p3);
            float t2area = TriangleGetArea(p1, p, p3);
            float t3area = TriangleGetArea(p1, p2, p);
            
            //3. Check if the main triangle area is the same as the 3 small triangle combined
            return Mathf.Approximately(mainarea, t1area + t2area + t3area) ? 1 : -1; // +1 or -2 , inside or outside
        }
        #endregion

        public static int IsPointInRect(Vector2 targetCenter, Vector2 size, float rotation, Vector2 point)
        {
            //todo

            float val = 0;

            if (val == 0) return 0;  // on rect
            return (int)Mathf.Sign(val); // +1 or -1 , inside or outside
        }

        /// <summary>
        /// Check to see if Point is within Rect on Rect or outside Rect - orentation method
        /// Uses IsLeft/Orientation checking 4 times
        /// if all is going left, then point is within rect, 
        /// checking for left or right depending on order of points supplied to isleft/orientation function
        /// </summary>
        public static int IsPointInRectOrientation(Vector2 targetCenter, Vector2 size, float rotation, Vector2 point)
        {
            //anticlockwise
            var p1 = targetCenter + new Vector2(-size.x, size.y) / 2;//tl
            var p2 = targetCenter + new Vector2(-size.x, -size.y) / 2;//bl
            var p3 = targetCenter + new Vector2(size.x, -size.y) / 2;//br
            var p4 = targetCenter + new Vector2(size.x, size.y) / 2;//tr

            //rotate points
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);
            p1 -= targetCenter;
            p2 -= targetCenter;
            p3 -= targetCenter;
            p4 -= targetCenter;
            p1 = new Vector2(p1.x * cos - p1.y * sin, p1.x * sin + p1.y * cos);
            p2 = new Vector2(p2.x * cos - p2.y * sin, p2.x * sin + p2.y * cos);
            p3 = new Vector2(p3.x * cos - p3.y * sin, p3.x * sin + p3.y * cos);
            p4 = new Vector2(p4.x * cos - p4.y * sin, p4.x * sin + p4.y * cos);
            p1 += targetCenter;
            p2 += targetCenter;
            p3 += targetCenter;
            p4 += targetCenter;

            float v1 = Orientation(p1, p2, point);
            float v2 = Orientation(p2, p3, point);
            float v3 = Orientation(p3, p4, point);
            float v4 = Orientation(p4, p1, point);

            if (v1 == -1 || v2 == -1 || v3 == -1 || v4 == -1) return -1;// outside rect
            return (int)(v1 * v2 * v3 * v4);// 0 or 1 , on rect or inside rect
        }
    }
}

