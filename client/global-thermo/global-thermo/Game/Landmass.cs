using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace global_thermo.Game
{
    class Landmass : Sprite
    {
        public List<Vector2> Points;

        public Landmass(GlobalThermoGame game, int radius, List<Vector2> points)
            : base(game)
        {
            Points = points;
            cameraEffect = new BasicEffect(game.GraphicsDevice);
        }

        public override void Render(Matrix transform)
        {
            base.Render(transform);

            Color color = new Color(90, 90, 90);

            int numpts = Points.Count + 1;
            VertexPositionColorTexture[] pointList = new VertexPositionColorTexture[numpts];
            pointList[0] = new VertexPositionColorTexture(new Vector3(rectPosition.X, rectPosition.Y, 0), color, new Vector2(0, 0));
            int i = 1;

            cameraEffect.World = Matrix.Identity;
            Matrix proj = Matrix.CreateOrthographicOffCenter(
                0,game.GraphicsManager.PreferredBackBufferWidth,game.GraphicsManager.PreferredBackBufferHeight,0,0,-1f) ;
            proj.Translation = transform.Translation;
            cameraEffect.View = Matrix.Identity;
            cameraEffect.Projection = proj;
            cameraEffect.VertexColorEnabled = true;

            //Console.WriteLine(transform);
            cameraEffect.CurrentTechnique.Passes[0].Apply();
            

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
