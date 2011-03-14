using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using global_thermo.Game.Pods;
using global_thermo.Game.Screens;
using Microsoft.Xna.Framework.Audio;

namespace global_thermo.Game
{
    public class Cursor : Sprite
    {
        public Vector2 GamePosition;
        public Cursor(GlobalThermoGame game, Screen screen)
            : base(game)
        {
            this.screen = screen;
        }

        public override void Initialize()
        {
            LoadTexture(game.Content.Load<Texture2D>("images/interface/cursor_base"));
            base.Initialize();
        }

        public override void Update(double deltaTime)
        {
            // Update position based on mouse
            mPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            GamePosition = screen.UIToGame(mPosition);
            SetTopLeft(new Vector2(Mouse.GetState().X - 4, Mouse.GetState().Y - 4));

            lastState = state;
            state = Mouse.GetState();

            base.Update(deltaTime);
        }

        protected MouseState lastState;
        protected MouseState state;
        protected Screen screen;
        protected Vector2 mPosition;
    }
}
