using System;

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
            using (GlobalThermoGame game = new GlobalThermoGame())
            {
                game.Run();
            }
        }
    }
#endif
}

