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

        public Camera(Vector2 center)
        {
            Center = center;
        }

        public Matrix GetTransform()
        {
            // Make an identity matrix, translate it, zoom it
            Matrix mtx = new Matrix();
            mtx.Translation = new Vector3(Center.X, Center.Y, 0);
            mtx = Matrix.Multiply(mtx, (float)Zoom);
            return mtx;
        }
    }
}
