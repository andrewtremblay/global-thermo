using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    public enum PodType
    {
        ResourceAny,
        ResourceG,
        ResourceA1,
        ResourceA2,
        ResourceA3,
        Residence,
        Defense,
        Branch
    }

    public abstract class Pod : IGameEntity
    {
        public const double Radius = 15;

        public int PodID;
        public Vector2D Position;
        public PodType Type;
        

        public Pod(Player owner, int podId, Vector2D position)
        {
            PodID = podId;
            Position = position;
            this.owner = owner;
            connectable = true;
        }

        public virtual void Connect()
        {
            connectable = false;
        }

        public virtual bool IsConnectable()
        {
            return connectable;
        }

        public abstract void Simulate(double timeDelta);

        protected bool connectable;
        protected Player owner;
    }
}
