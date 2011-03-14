using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace global_thermo.Game.Interface
{
    // Buttons are assumed to be 3-frame spritesheets: frame 0 = normal, 1 = mouseover, 2 = mousedown
    public class Button : Sprite
    {
        public Button(GlobalThermoGame game, Action callback)
            : base(game)
        {
            this.callback = callback;
            pressed = false;
        }

        public override void Initialize()
        {
            rolloverSnd = game.Content.Load<SoundEffect>("sounds/rollover");
            clickSnd = game.Content.Load<SoundEffect>("sounds/click");
            base.Initialize();
        }

        protected override void updateSelf(double deltaTime)
        {
            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;
            Vector2 halfSize = size / 2;

            // This is the collision check of the mouse cursor over the button rectangle
            if (mx >= (RectPosition - halfSize).X && mx < (RectPosition + halfSize).X &&
                my >= (RectPosition - halfSize).Y && my < (RectPosition + halfSize).Y)
            {
                // Record if you've clicked on the button so that we can check later if you let go of the mouse on the button or not
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (Frame == 1) { PlayClick(); }
                    Frame = 2;
                    pressed = true;
                }
                else
                {
                    if (Frame == 0) { PlayRollover(); }
                    Frame = 1;
                }
            }
            else
            {
                Frame = 0;
                pressed = false;
            }

            // If you let go of the mouse on the button, perform the action
            if (pressed && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                callback.Invoke();
                pressed = false;
            }
        }

        private Action callback;
        private bool pressed;

        public void PlayRollover() { rolloverSnd.Play((float)(game.Rand.NextDouble() * 0.1 + 0.15), (float)(game.Rand.NextDouble() * 0.05), 0.0f); }
        public void PlayClick() { clickSnd.Play(0.5f, (float)(game.Rand.NextDouble() * 0.1), 0.0f); }

        protected SoundEffect rolloverSnd;
        protected SoundEffect clickSnd;
    }
}
