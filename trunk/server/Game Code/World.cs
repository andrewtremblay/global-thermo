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
        public List<Vector2D> Landmass;

        public World(IEnumerable<Player> players)
        {
            GameTime = 0;
            IsGameRunning = true;
            Players = new List<Player>(players);
            Atmospheres = new List<Atmosphere>();
            WorldLava = new Lava(300);
            Landmass = new List<Vector2D>();
            generateLandmass();
        }

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

        private void generateLandmass()
        {
            double angle = 0;
            double dist = 300;
            Random rand = new Random();

            // Goes around in a circle and generates points for the landmass
            for (angle = 0; angle < Math.PI * 2 - 0.05; angle += rand.NextDouble() * 0.05 + 0.05)
            {
                // Chooses a new height for the land based on the previous height with min and max values
                dist = Math.Max(Math.Min(dist + rand.NextDouble() * 20 - 10, WorldLava.Height + 80), WorldLava.Height - 80);
                double x = Math.Sin(angle) * dist;
                double y = -Math.Cos(angle) * dist;
                Vector2D pt = new Vector2D(x, y);
                Console.WriteLine(pt);
                Landmass.Add(pt);
            }
            Landmass.Add(Landmass[0]);
        }
    }
}
