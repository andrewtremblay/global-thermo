using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public abstract class Pod : IGameEntity
    {
        public int PodID;
        public Vector2D Position;

        abstract public void Simulate(double timeDelta);


    }
}
