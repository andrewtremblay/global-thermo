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
        Place,
        Inspect
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

            pResidence = game.Content.Load<Texture2D>("images/gameplay/blueprint_residence");
            pResourceA1 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo1");
            pResourceA2 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo2");
            pResourceA3 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo3");
            pResourceG = game.Content.Load<Texture2D>("images/gameplay/blueprint_ground");
            pBranch = game.Content.Load<Texture2D>("images/gameplay/blueprint_branch");
            pDefense = game.Content.Load<Texture2D>("images/gameplay/blueprint_defense");
            constructIcon = null;
        }

        public override void Update(double deltaTime)
        {
            // Update position based on mouse
            mPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            SetTopLeft(new Vector2(Mouse.GetState().X - 4, Mouse.GetState().Y - 4));

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
                case CursorMode.Inspect:
                    updateInspect(deltaTime);
                    break;
            }

            lastState = state;
            state = Mouse.GetState();

            base.Update(deltaTime);
        }

        public void PlacePodMode(PodType podType)
        {
            cursorMode = CursorMode.Place;
            placePodType = podType;
            switch (placePodType)
            {
                case PodType.Branch: constructIcon = pBranch; break;
                case PodType.Residence: constructIcon = pResidence; break;
                case PodType.Defense: constructIcon = pDefense; break;
                case PodType.ResourceAny: constructIcon = pResourceG; break;
            }
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
            SpriteColor = Color.White;  // TEMP
        }

        private void updatePlace(double deltaTime)
        {
            SpriteColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);

            if (JustClicked())
            {
                NetManager.GetInstance().SendPlacePodMessage(screen.UIToGame(mPosition), placePodType);
                cursorMode = CursorMode.Select;
            }
        }

        private void updateInspect(double deltaTime)
        {

        }

        protected override void renderSelf(Matrix transform)
        {
            if (cursorMode == CursorMode.Place && constructIcon != null)
            {
                game.batch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, transform);
                {
                    game.batch.Draw(constructIcon, new Rectangle((int)mPosition.X - constructIcon.Width / 2, (int)mPosition.Y - constructIcon.Height / 2, constructIcon.Width, constructIcon.Height), Color.White);
                }
                game.batch.End();
            }
            base.renderSelf(transform);
        }

        private int lastScroll;
        private CursorMode cursorMode;
        private MouseState lastState;
        private MouseState state;
        private Screen screen;
        private PodType placePodType;
        private Vector2 mPosition;

        private Texture2D constructIcon;
        // Hack maybe.
        private Texture2D pResourceG;
        private Texture2D pResourceA1;
        private Texture2D pResourceA2;
        private Texture2D pResourceA3;
        private Texture2D pResidence;
        private Texture2D pBranch;
        private Texture2D pDefense;

    }
}
