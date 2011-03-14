using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace global_thermo.Game
{
    public class StringSprite : Sprite
    {
        public String Text;
        public Color TextColor;
        public StringSprite(GlobalThermoGame game, String text)
            : base(game)
        {
            Text = text;
            TextColor = new Color(90, 90, 90);
        }

        public override void Initialize()
        {
            font = game.Content.Load<SpriteFont>("fonts/Courier New");
            base.Initialize();
        }

        public Vector2 GetSize()
        {
            return font.MeasureString(Text);
        }

        public override void SetTopLeft(Vector2 topLeft)
        {
            RectPosition = topLeft + font.MeasureString(Text) / 2;
        }

        protected override void renderSelf(Matrix transform)
        {
            if (Visible)
            {
                game.batch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, transform);
                {
                    game.batch.DrawString(font, Text, RectPosition - font.MeasureString(Text) / 2, TextColor);
                }
                game.batch.End();
            }
        }

        private SpriteFont font;
    }
}
