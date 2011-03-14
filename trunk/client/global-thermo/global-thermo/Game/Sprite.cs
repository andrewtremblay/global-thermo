using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace global_thermo.Game
{
    public class Sprite : GameObject
    {

        public Color SpriteColor;
        public int Frame;
        public double Angle;
        public double Scale;

        public Sprite(GlobalThermoGame game)
            : base(game)
        {
            texture = null;
            SpriteColor = Color.White;
            Angle = 0;
            Scale = 1.0;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
            size = new Vector2(texture.Width, texture.Height);
            this.frameWidth = 0;
            Frame = 0;
        }

        public void LoadTexture(Texture2D texture, int frameWidth)
        {
            LoadTexture(texture);
            this.frameWidth = frameWidth;
            size = new Vector2(frameWidth, texture.Height);
            Frame = 0;
        }

        public virtual void SetTopLeft(Vector2 topLeft)
        {
            rectPosition = topLeft + new Vector2(texture.Width / 2, texture.Height / 2);
        }

        protected override void renderSelf(Matrix transform)
        {
            if (texture != null && Visible)
            {
                game.batch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, transform);
                {
                    Vector2 halfSize = size / 2 * (float)Scale;
                    if (Angle == 0)
                    {
                        game.batch.Draw(texture, new Rectangle((int)rectPosition.X - (int)halfSize.X, (int)rectPosition.Y - (int)halfSize.Y, (int)(size.X * Scale), (int)(size.Y * Scale)),
                                    new Rectangle(Frame * (int)size.X, 0, (int)size.X, (int)size.Y), SpriteColor);
                    }
                    else
                    {
                        game.batch.Draw(texture, new Rectangle((int)rectPosition.X, (int)rectPosition.Y, (int)size.X, (int)size.Y),
                                    new Rectangle(Frame * (int)size.X, 0, (int)size.X, (int)size.Y), SpriteColor, (float)Angle, new Vector2(halfSize.X, halfSize.Y), SpriteEffects.None, 0);
                    }
                }
                game.batch.End();
            }
        }

        protected Texture2D texture;
        protected int frameWidth;
    }
}
