using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace global_thermo.Game
{
    public class ClickableStringSprite : StringSprite
    {
        public ClickableStringSprite(GlobalThermoGame game, String text, Action callback)
            : base(game, text)
        {
            this.callback = callback;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            Vector2 m = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (m.X >= rectPosition.X - GetSize().X / 2 && m.X < rectPosition.X + GetSize().X / 2 &&
                m.Y >= rectPosition.Y - GetSize().Y / 2 && m.Y < rectPosition.Y + GetSize().Y / 2)
            {
                TextColor = new Color(68, 225, 97);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    callback.Invoke();
                }
            }
            else
            {   
                TextColor = new Color(90, 90, 90);
            }
        }

        private Action callback;
    }
}
