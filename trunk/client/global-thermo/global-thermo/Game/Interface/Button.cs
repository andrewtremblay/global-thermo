using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Interface
{
    public class Button : Sprite
    {
        public Button(GlobalThermoGame game, Action callback)
            : base(game)
        {
            this.callback = callback;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;
            Vector2 halfSize = size / 2;
            Frame = 0;
            if (mx >= (RectPosition - halfSize).X && mx < (RectPosition + halfSize).X &&
                my >= (RectPosition - halfSize).Y && my < (RectPosition + halfSize).Y)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Frame = 2;
                }
                else
                {
                    Frame = 1;
                }

            }

            if (Frame == 2 && Mouse.GetState().LeftButton == ButtonState.Released)
            {

            }
        }

        private Action callback;
    }
}
