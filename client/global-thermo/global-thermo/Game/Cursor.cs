using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace global_thermo.Game
{
    class Cursor : Sprite
    {
        public Cursor(GlobalThermoGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            LoadTexture(game.Content.Load<Texture2D>("images/interface/cursor_base"));
        }

        public override void Update(double deltaTime)
        {
            SetTopLeft(new Vector2(Mouse.GetState().X + 4, Mouse.GetState().Y + 4));
            base.Update(deltaTime);
        }
    }
}
