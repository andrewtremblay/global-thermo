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
using global_thermo.Game.Screen;

namespace global_thermo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GlobalThermoGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager Graphics;

        public GlobalThermoGame()
        {
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(this);
        }

        public void SetScreen(Screen screen)
        {
            this.screen = screen;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (screen != null)
            {
                screen.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (screen != null)
            {
                screen.Render();
            }
            base.Draw(gameTime);
        }

        protected Screen screen;
    }
}
