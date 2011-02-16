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

        public Pod(Player owner, int podId, Vector2D position)
        {
            PodID = podId;
            Position = position;
            this.owner = owner;
        }

        public abstract void Simulate(double timeDelta);

        private Player owner;
    }
}
