using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using global_thermo.Game.Screens;

namespace global_thermo
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GlobalThermoGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager GraphicsManager;
        public SpriteBatch batch;

        public GlobalThermoGame()
        {
            Content.RootDirectory = "Content";
            GraphicsManager = new GraphicsDeviceManager(this);
            GraphicsManager.PreferMultiSampling = true;
            screen = null;
        }

        public void SetScreen(Screen screen)
        {
            if (this.screen != null)
            {
                // previous screen clean-up?
                this.screen = screen;
                screen.Initialize();
            }
            else
            {
                this.screen = screen;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            batch = new SpriteBatch(GraphicsDevice);
            screen.Initialize();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (screen != null && screen.Initialized)
            {
                screen.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(247,247,247));
            if (screen != null && screen.Initialized)
            {
                screen.Render(Matrix.Identity);
            }
            base.Draw(gameTime);
        }

        protected Screen screen;
    }
}
