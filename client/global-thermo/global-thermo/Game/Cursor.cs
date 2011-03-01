using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using global_thermo.Game.Pods;
using global_thermo.Game.Screens;

namespace global_thermo.Game
{
    enum CursorMode
    {
        Select,
        Place
    }

    public class Cursor : Sprite
    {
        public double ZoomLevel;
        public Cursor(GlobalThermoGame game, Screen screen)
            : base(game)
        {
            cursorMode = CursorMode.Select;
            this.screen = screen;
            ZoomLevel = 1.0;
        }

        public override void Initialize()
        {
            base.Initialize();
            LoadTexture(game.Content.Load<Texture2D>("images/interface/cursor_base"));
        }

        public override void Update(double deltaTime)
        {
            // Update position based on mouse
            SetTopLeft(new Vector2(Mouse.GetState().X + 4, Mouse.GetState().Y + 4));

            int scrollDelta = state.ScrollWheelValue - lastScroll;
            lastScroll = state.ScrollWheelValue;

            ZoomLevel = Math.Min(Math.Max(ZoomLevel - scrollDelta / 2400.0, 0.125), 1);

            // Cursor modes
            switch (cursorMode)
            {
                case CursorMode.Select:
                    updateSelect(deltaTime);
                    break;
                case CursorMode.Place:
                    updatePlace(deltaTime);
                    break;
            }

            lastState = state;
            state = Mouse.GetState();

            // DEBUG, REMOVE LOL!?!!?!?
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                cursorMode = CursorMode.Place;
                placePodType = PodType.Resource;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cursorMode = CursorMode.Place;
                placePodType = PodType.Residence;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                cursorMode = CursorMode.Place;
                placePodType = PodType.Defense;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                cursorMode = CursorMode.Place;
                placePodType = PodType.Branch;
            }
            // END REMOVE LOL!?!?!?!

            base.Update(deltaTime);
        }

        public Boolean JustClicked()
        {
            if (lastState.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        private void updateSelect(double deltaTime)
        {
            // TEMP
            SpriteColor = Color.White;
        }

        private void updatePlace(double deltaTime)
        {
            // TEMP
            SpriteColor = Color.Green;
            if (JustClicked())
            {
                NetManager.GetInstance().SendPlacePodMessage(screen.UIToGame(rectPosition), placePodType);
                cursorMode = CursorMode.Select;
            }
        }

        private int lastScroll;
        private CursorMode cursorMode;
        private MouseState lastState;
        private MouseState state;
        private Screen screen;
        private PodType placePodType;

    }
}
