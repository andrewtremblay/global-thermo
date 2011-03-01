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
            //mtx = Matrix.Multiply(mtx, (float)Zoom);
            //Matrix mtx = Matrix.CreateScale((float)Zoom, (float)Zoom, 1);

            mtx.Translation = new Vector3(-Center.X + windowSize.X / 2 / (float)Zoom, -Center.Y + windowSize.Y / 2 / (float)Zoom, 0);
            mtx.M44 = 1.0f / (float)Zoom;
            //mtx.M12 = (float)Zoom;

            //Console.WriteLine(mtx);
            
            return mtx;
        }

        protected Vector2 windowSize;
    }
}
