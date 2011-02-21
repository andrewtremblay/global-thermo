using System;
using global_thermo.Game.Screen;

namespace global_thermo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            GlobalThermoGame game = null;
            game = new GlobalThermoGame();
            game.SetScreen(new TitleScreen(game));
            game.Run();
        }
    }
#endif
}

