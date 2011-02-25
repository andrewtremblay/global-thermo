using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    public enum PodType
    {
        Residence,
        Resource,
        Defense,
        Branch
    }

    public abstract class Pod : IGameEntity
    {
        public const double Radius = 15;

        public int PodID;
        public Vector2D Position;
        public PodType Type;
        public bool Connectable;

        public Pod(Player owner, int podId, Vector2D position)
        {
            PodID = podId;
            Position = position;
            this.owner = owner;
            Connectable = true;
        }

        public abstract void Simulate(double timeDelta);

        protected Player owner;
    }
}
