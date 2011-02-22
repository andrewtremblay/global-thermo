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

        public Camera(Vector2 center, Vector2 windowSize)
        {
            Center = center;
            Zoom = 1.0;
            this.windowSize = windowSize;
        }

        public Matrix GetTransform()
        {
            // Make an identity matrix, translate it, zoom it
            Matrix mtx = Matrix.Identity;
            mtx.Translation = new Vector3(Center.X - windowSize.X / 2, Center.Y - windowSize.Y / 2, 0);
            mtx = Matrix.Multiply(mtx, (float)Zoom);
            return mtx;
        }

        protected Vector2 windowSize;
    }
}
