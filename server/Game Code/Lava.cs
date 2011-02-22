using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public class Lava : IGameEntity
    {
        public double Height;

        public Lava(double height)
        {
            Height = height;
        }

        public void Simulate(double timeDelta)
        {

        }
    }
}
