using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace global_thermo.Game
{
    class Planet : Sprite
    {
        public List<Vector2> Points;
        public Vector2 Center;
        public double LavaRadius;
        public double WaterRadius;

        public Planet(GlobalThermoGame game, Vector2 center, List<Vector2> points)
            : base(game)
        {
            Center = center;
            Points = points;
            cameraEffect = new BasicEffect(game.GraphicsDevice);
            LavaRadius = 0;
            WaterRadius = 0;
        }

        public override void Render(Matrix transform)
        {
            base.Render(transform);

            float screenW = game.GraphicsManager.PreferredBackBufferWidth;
            float screenH = game.GraphicsManager.PreferredBackBufferHeight;

            cameraEffect.World = Matrix.Identity;
            Matrix proj = Matrix.CreateOrthographicOffCenter(
                0,screenW, screenH,0,0,-1f) ;
            Vector3 translation = new Vector3((transform.Translation.X - screenW / 2 * transform.M44) / (screenW / 2), -(transform.Translation.Y - screenH / 2 * transform.M44) / (screenH / 2), 0);

            proj.Translation = translation;
            proj.M44 = transform.M44;
            cameraEffect.View = Matrix.Identity;
            cameraEffect.Projection = proj;
            cameraEffect.VertexColorEnabled = true;

            cameraEffect.CurrentTechnique.Passes[0].Apply();


            renderWater();
            renderLandmass();
            renderLava();
        }

        private void renderLava()
        {
            renderCircle1(LavaRadius, new Color(255, 0, 0));
        }

        private void renderWater()
        {
            renderCircle1(WaterRadius, new Color(0, 0, 255));
        }

        private void renderCircle1(double radius, Color color)
        {
            int numPts = 128;
            VertexPositionColorTexture[] pointList = new VertexPositionColorTexture[numPts];
            int[] circleIndices = new int[numPts + 1];
            int i = 0;
            for (i = 0; i < numPts; i++)
            {
                double angle = (Math.PI * 2 / numPts) * i;
                int x = (int)(Math.Cos(angle) * radius);
                int y = (int)(-Math.Sin(angle) * radius);
                pointList[i] = new VertexPositionColorTexture(new Vector3(x,y,0), color, new Vector2(0,0));
                circleIndices[i] = i;
            }
            circleIndices[i] = 0;
            game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                PrimitiveType.LineStrip,
                pointList,
                0,
                numPts,
                circleIndices,
                0,
                numPts);
        }

        private void renderLandmass()
        {
            Color color = new Color(90, 90, 90);

            int numpts = Points.Count + 1;
            VertexPositionColorTexture[] pointList = new VertexPositionColorTexture[numpts];
            pointList[0] = new VertexPositionColorTexture(new Vector3(rectPosition.X, rectPosition.Y, 0), color, new Vector2(0, 0));
            int i = 1;

            foreach (Vector2 pt in Points)
            {
                pointList[i] = new VertexPositionColorTexture(new Vector3(pt.X, pt.Y, 0), color, new Vector2(0, 0));
                i++;
            }

            // Initialize an array of indices of type short.
            int[] triangleListIndices = new int[numpts * 3];
            // Populate the array with references to indices in the vertex buffer
            for (i = 0; i < numpts; i++)
            {
                triangleListIndices[i * 3] = 0;
                triangleListIndices[(i * 3) + 1] = (int)(i + 1);
                triangleListIndices[(i * 3) + 2] = (int)(i + 2);

            }

            triangleListIndices[(numpts * 3) - 1] = 1;

            game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    numpts,  // number of vertices to draw
                    triangleListIndices,
                    0,  // first index element to read
                    numpts - 1   // number of primitives to draw
            );
        }

        private BasicEffect cameraEffect;
    }
}
