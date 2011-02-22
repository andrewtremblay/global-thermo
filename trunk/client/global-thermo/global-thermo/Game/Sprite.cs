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

        public bool Visible;

        public Sprite(GlobalThermoGame game)
            : base(game)
        {
            texture = null;
            Visible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void SetTopLeft(Vector2 topLeft)
        {
            rectPosition = topLeft + new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public override void Render(Matrix transform)
        {
            base.Render(transform);
            if (texture != null && Visible)
            {
                game.batch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, transform);
                {
                    game.batch.Draw(texture, new Rectangle((int)rectPosition.X - texture.Width / 2, (int)rectPosition.Y - texture.Height / 2, texture.Width, texture.Height), Color.White);
                }
                game.batch.End();
            }
        }

        private Texture2D texture;
    }
}
