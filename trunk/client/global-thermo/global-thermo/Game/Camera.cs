using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace global_thermo.Game
{
    public class Camera
    {
        public Vector2 Center;
        public double Zoom;
        public double Angle;

        public Camera(Vector2 center, Vector2 windowSize)
        {
            Center = center;
            Zoom = 1.0;
            Angle = 0.0;
            this.windowSize = windowSize;
        }

        public Matrix GetTransform()
        {
            Matrix mtx = Matrix.CreateRotationZ((float)Angle) * Matrix.CreateTranslation(-Center.X, -Center.Y, 0) * Matrix.CreateScale((float)Zoom);
            mtx *= Matrix.CreateTranslation(windowSize.X / 2, windowSize.Y / 2, 0);
            return mtx;
        }

        protected Vector2 windowSize;
    }
}
