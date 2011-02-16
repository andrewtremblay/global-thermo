using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public class World : IGameEntity
    {
        public double GameTime;
        public bool IsGameRunning;
        public List<Player> Players;
        public List<Atmosphere> Atmospheres;
        public Lava WorldLava;

        public void Simulate(double timeDelta)
        {
            foreach (Player player in Players)
            {
                player.Simulate(timeDelta);
            }
            foreach (Atmosphere atmo in Atmospheres)
            {
                atmo.Simulate(timeDelta);
            }
            GameTime += timeDelta;
        }
    }
}
