using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Screen
{
    public abstract class Screen : GameObject
    {
        public Camera UICamera;
        public Camera GameCamera;

        public Screen(GlobalThermoGame game)
            : base(game)
        {
            width = game.Graphics.PreferredBackBufferWidth;
            height = game.Graphics.PreferredBackBufferHeight;
            UICamera = new Camera(new Vector2(width / 2, height / 2));
            GameCamera = new Camera(new Vector2(0, 0));
        }

        protected int width;
        protected int height;
    }
}
