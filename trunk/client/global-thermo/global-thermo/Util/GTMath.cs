using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace global_thermo.Util
{
    public class GTMath
    {
        public static Vector2 RectToPolar(Vector2 rect)
        {
            float angle = (float)(Math.Atan2(rect.Y, rect.X) + Math.PI / 2);
            float magnitude = rect.Length();
            return new Vector2(angle, magnitude);
        }

        public static Vector2 PolarToRect(Vector2 polar)
        {
            float x = (float)Math.Sin(polar.X) * polar.Y;
            float y = (float)-Math.Cos(polar.X) * polar.Y;
            return new Vector2(x, y);
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float val)
        {
            float x = (b.X - a.X) * val + a.X;
            float y = (b.Y - a.Y) * val + a.Y;
            return new Vector2(x, y);
        }

        public static double SqDistanceToLineSegment(Vector2 pt, Vector2 lpt1, Vector2 lpt2, out Vector2 linePt)
        {
            double r_numerator = (pt.X-lpt1.X)*(lpt2.X-lpt1.X) + (pt.Y-lpt1.Y)*(lpt2.Y-lpt1.Y);
            double r_denomenator = (lpt2.X-lpt1.X)*(lpt2.X-lpt1.X) + (lpt2.Y-lpt1.Y)*(lpt2.Y-lpt1.Y);
            //Console.WriteLine(r_denomenator);
            double r = r_numerator / r_denomenator;

            double px = lpt1.X + r*(lpt2.X-lpt1.X);
            double py = lpt1.Y + r*(lpt2.Y-lpt1.Y);

            double s =  ((lpt1.Y-pt.Y)*(lpt2.X-lpt1.X)-(lpt1.X-pt.X)*(lpt2.Y-lpt1.Y) ) / r_denomenator;

            double distanceLine = Math.Abs(s)*r_denomenator;
            double distanceSegment = 0.0;
 
            // (xx,yy) is the point on the lineSegment closest to (pt.X,pt.Y)
            double xx = px;
            double yy = py;


            if ( (r >= 0) && (r <= 1) )
            {
                    distanceSegment = distanceLine;
            }
            else
            {
                double dist1 = (pt.X-lpt1.X)*(pt.X-lpt1.X) + (pt.Y-lpt1.Y)*(pt.Y-lpt1.Y);
                double dist2 = (pt.X-lpt2.X)*(pt.X-lpt2.X) + (pt.Y-lpt2.Y)*(pt.Y-lpt2.Y);
                if (dist1 < dist2)
                {
                        xx = lpt1.X;
                        yy = lpt1.Y;
                        distanceSegment = dist1;
                }
                else
                {
                        xx = lpt2.X;
                        yy = lpt2.Y;
                        distanceSegment = dist2;
                }
            }
            linePt = new Vector2((float)xx, (float)yy);
            return distanceSegment;
        }
        public static double SqDistanceToLineSegment(Vector2 pt, Vector2 lpt1, Vector2 lpt2)
        {
            Vector2 junk;
            return SqDistanceToLineSegment(pt, lpt1, lpt2, out junk);
        }

        public static double AngleDifference(double a1, double a2)
        {
            double difference = a2 - a1;
            while (difference < -Math.PI/2) difference += (Math.PI*2);
            while (difference > Math.PI) difference -= (Math.PI * 2);
            return difference;
        }
    }
}
