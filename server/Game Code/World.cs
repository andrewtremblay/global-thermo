using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalThermo.Pods;

namespace GlobalThermo
{
    public class World : IGameEntity
    {
        const double MAXGAMESPEED = 100.0;
        const double MINGAMESPEED = 1.0;

        public GameCode Game;
        public double GameSpeed;
        public double GameTime;
        public bool IsGameRunning;
        public List<Player> Players;
        public List<Atmosphere> Atmospheres;
        public double LavaHeight;
        public double LavaHeightDelta;
        public double WaterHeight;
        public double WaterHeightDelta;
        public double TrenchHeight;
        public double BoilThreshold;
        public double Unreplenishable;
        public List<Vector2D> Landmass;

        public World(GameCode game)
        {
            Game = game;
            GameTime = 0;
            GameSpeed = 1.0;
            IsGameRunning = true;
            Players = new List<Player>();
            Atmospheres = new List<Atmosphere>();
            Atmospheres.Add(new Atmosphere(1200, 2400, ResourceType.Atmo1, PodType.ResourceA1));
            Atmospheres.Add(new Atmosphere(2400, 3000, ResourceType.Atmo2, PodType.ResourceA2));
            Atmospheres.Add(new Atmosphere(3000, 3600, ResourceType.Atmo3, PodType.ResourceA3));
            LavaHeight = 800;
            WaterHeight = 1800;
            Unreplenishable = 0;
            waterMax = WaterHeight;
            TrenchHeight = WaterHeight - 180;
            BoilThreshold = TrenchHeight - 250;
            minLavaHeight = LavaHeight;
            Landmass = new List<Vector2D>();
            generateLandmass();
        }

        public void Simulate(double timeDelta)
        {
            timeDelta *= GameSpeed;

            Dictionary<ResourceType, int> collectors = new Dictionary<ResourceType, int>();
            collectors[ResourceType.Atmo1] = 0;
            collectors[ResourceType.Atmo2] = 0;
            collectors[ResourceType.Atmo3] = 0;
            collectors[ResourceType.Ground] = 0;
            // Update the players and count the resource collectors
            foreach (Player player in Players)
            {
                player.Simulate(timeDelta);
                foreach (Pod p in player.Pods)
                {
                    if (p is ResourcePod)
                    {
                        collectors[(p as ResourcePod).RType]++;
                    }
                }
            }
            foreach (Atmosphere atmo in Atmospheres)
            {
                atmo.Simulate(timeDelta);
            }

            // When the lava reaches the (arbitrary) boiling threshold, the water starts evaporating
            // Between boiling threshold and the bottom of the water (trench), the water will evaporate 50%,
            // with no permanent damage. After the lava reaches the trench, it starts doing permanent damage,
            // preventing the water from returning fully. If the lava overtakes the water completely, the water is 100%
            // gone permanently. 

            // Figure out how much the lava should move
            double lavaRate = 0.1;
            double diminish = ((waterMax - LavaHeight) / waterMax) * 1.5 + 0.1; // This makes it grow slower as the lava level rises
            //LavaHeightDelta = (-10.0 + collectors[ResourceType.Atmo1] + collectors[ResourceType.Atmo2] * 2 + collectors[ResourceType.Atmo3] * 3) * lavaRate * timeDelta;

            LavaHeightDelta = 100 * timeDelta; // As a test of the water/lava interactions

            LavaHeight = Math.Max(minLavaHeight, LavaHeight + LavaHeightDelta * diminish);

            if (LavaHeight >= BoilThreshold)
            {
                if (LavaHeight < TrenchHeight)
                {
                    WaterHeight = Math.Min(waterMax - ((waterMax - TrenchHeight) * ((LavaHeight - BoilThreshold) / (TrenchHeight - BoilThreshold)) * 0.5), WaterHeight);
                }
                else
                {
                    double waterHalf = TrenchHeight + ((waterMax - TrenchHeight) / 2);
                    Unreplenishable = Math.Max(Unreplenishable, 1 - ((waterHalf - LavaHeight) / (waterHalf - TrenchHeight)));
                }
            }
            else
            {
                if (WaterHeight < waterMax)
                {
                    WaterHeight += timeDelta * waterRegenRate;
                }
            }
            if (LavaHeight >= 1660) { LavaHeight = 0; }
            WaterHeight = Math.Min(WaterHeight, waterMax - (waterMax - TrenchHeight) * Unreplenishable);

            GameTime += timeDelta;
        }

        public void CalcGameSpeed()
        {
            GameSpeed = MAXGAMESPEED;
            foreach (Player player in Players)
            {
                if (player.GetVoteSpeed() < GameSpeed)
                {
                    GameSpeed = Math.Max(player.GetVoteSpeed(), MINGAMESPEED);
                }
            }
            Game.Broadcast("GameSpeed", GameSpeed);
        }

        private void generateLandmass()
        {
            double angle = 0;
            double dist = WaterHeight;
            Random rand = new Random();

            // Goes around in a circle and generates points for the landmass
            for (angle = 0; angle < Math.PI * 2 - 0.005; angle += rand.NextDouble() * 0.005 + 0.005)
            {
                // Chooses a new height for the land based on the previous height with min and max values
                dist = Math.Max(Math.Min(dist + rand.NextDouble() * 60 - 30, WaterHeight + 120), TrenchHeight);
                double x = Math.Sin(angle) * dist;
                double y = -Math.Cos(angle) * dist;
                Vector2D pt = new Vector2D(x, y);
                Landmass.Add(pt);
            }
            Landmass.Add(Landmass[0]);
        }

        private double minLavaHeight;
        private double waterMax;
        private double waterRegenRate = 10.0;
    }
}

