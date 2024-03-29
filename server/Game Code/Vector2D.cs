﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GlobalThermo
{
    /// <summary>
    /// Summary description for Vector2D.
    /// </summary>
    public struct Vector2D
    {
        private double x, y;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #region Properties
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        #endregion

        #region Object
        public override bool Equals(object obj)
        {
            if (obj is Vector2D)
            {
                Vector2D v = (Vector2D)obj;
                if (v.x == x && v.y == y)
                    return obj.GetType().Equals(this.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{{X={0}, Y={1}}}", x, y);
        }
        #endregion

        public double Norm()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static bool operator ==(Vector2D u, Vector2D v)
        {
            if (u.X == v.x && u.y == v.y)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vector2D u, Vector2D v)
        {
            return u != v;
        }

        public static Vector2D operator +(Vector2D u, Vector2D v)
        {
            return new Vector2D(u.x + v.x, u.y + v.y);
        }

        public static Vector2D operator -(Vector2D u, Vector2D v)
        {
            return new Vector2D(u.x - v.x, u.y - v.y);
        }

        public static Vector2D operator *(Vector2D u, double a)
        {
            return new Vector2D(a * u.x, a * u.y);
        }

        public static Vector2D operator /(Vector2D u, double a)
        {
            return new Vector2D(u.x / a, u.y / a);
        }

        public static Vector2D operator -(Vector2D u)
        {
            return new Vector2D(-u.x, -u.y);
        }

        public Vector2D ToPolar()
        {
            double angle = Math.Atan2(Y, X);
            return new Vector2D(angle, Magnitude());
        }

        public Vector2D ToRect()
        {
            double x = Math.Sin(X) * Y;
            double y = -Math.Cos(X) * Y;
            return new Vector2D(x, y);
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double MagnitudeSquared()
        {
            return x * x + y * y;
        }

        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }
    }
}
