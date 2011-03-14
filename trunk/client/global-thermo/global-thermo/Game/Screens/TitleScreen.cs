using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace global_thermo.Game.Screens
{
    class TitleScreen : Screen
    {
        public TitleScreen(GlobalThermoGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            
            background = new Sprite(game);
            background.LoadTexture(game.Content.Load<Texture2D>("images/interface/titlescreen"));
            background.SetTopLeft(new Vector2(0,0));
            InterfaceChildren.Add(background);

            menuHighlight = new Sprite(game);
            menuHighlight.LoadTexture(game.Content.Load<Texture2D>("images/interface/title_menuhighlight"));
            menuHighlight.SetTopLeft(new Vector2(565, 292));
            InterfaceChildren.Add(menuHighlight);

            menu = new Sprite(game);
            menu.LoadTexture(game.Content.Load<Texture2D>("images/interface/title_menu"));
            menu.SetTopLeft(new Vector2(575, 300));
            InterfaceChildren.Add(menu);

            cursor = new Cursor(game, this);
            InterfaceChildren.Add(cursor);

            rolloverSnd = game.Content.Load<SoundEffect>("sounds/rollover");
            clickSnd = game.Content.Load<SoundEffect>("sounds/click");

            base.Initialize();
            // Test
            GameCamera.Angle = 0.5;
            Console.WriteLine(GameCamera.GetTransform());
            //Console.WriteLine(Matrix.CreateOrthographicOffCenter(0, 800, 600, 0, 0, -1f));
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            updateMenuOptions();
        }

        private void updateMenuOptions()
        {
            // Menu Highlight position
            // Mouse control
            float y = cursor.RectPosition.Y - menu.RectPosition.Y;

            int oldOption = menuOption;

            if (y < -60) { menuOption = 0; }
            else if (y < 8) { menuOption = 1; }
            else if (y < 84) { menuOption = 2; }
            else { menuOption = 3; }

            if (menuOption != oldOption) { playRollover(); }

            switch (menuOption)
            {
                case 0: y = -101; break;
                case 1: y = -32; break;
                case 2: y = 40; break;
                case 3: y = 114; break;
            }
            menuHighlight.RectPosition = new Vector2(menuHighlight.RectPosition.X, y + menu.RectPosition.Y);

            // Handle clicking
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                playClick();
                if (menuOption == 0)
                {
                    game.SetScreen(new JoinGameScreen(game));
                }
                if (menuOption == 1)
                {
                    game.SetScreen(new HostGameScreen(game));
                }
            }
        }

        private void playRollover() { rolloverSnd.Play((float)(game.Rand.NextDouble() * 0.1 + 0.15), (float)(game.Rand.NextDouble() * 0.05), 0.0f); }
        private void playClick() { clickSnd.Play(0.5f, (float)(game.Rand.NextDouble() * 0.1), 0.0f); }

        private Sprite menuHighlight;
        private Sprite background;
        private Sprite menu;
        private Cursor cursor;
        private int menuOption;

        private SoundEffect clickSnd;
        private SoundEffect rolloverSnd;

    }
}


