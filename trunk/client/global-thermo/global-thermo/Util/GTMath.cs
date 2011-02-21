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
            float angle = (float)Math.Atan2(rect.Y, rect.X);
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
    }
}
