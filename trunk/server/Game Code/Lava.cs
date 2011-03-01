using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public class Lava : IGameEntity
    {
        public double Height;
        public double RisingRate;
        public double FallingRate;

        public Lava(double height)
        {
            minHeight = height;
            Height = height;
            FallingRate = 20.0;
            RisingRate = 0.0;
        }

        public void Simulate(double timeDelta)
        {
            Height = Math.Max(minHeight, Height + (RisingRate - FallingRate) * timeDelta);
        }

        public double minHeight;
    }
}
