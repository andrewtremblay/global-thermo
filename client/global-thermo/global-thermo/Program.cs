using System;
using global_thermo.Game.Screens;

namespace global_thermo
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            GlobalThermoGame game = null;
            game = new GlobalThermoGame();
            game.GraphicsManager.PreferredBackBufferWidth = 800;
            game.GraphicsManager.PreferredBackBufferHeight = 600;
            game.SetScreen(new TitleScreen(game));
            game.Run();
        }
    }
#endif
}

