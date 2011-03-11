using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace global_thermo.Game.Pods
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

    public abstract class Pod : Sprite
    {
        public int PodID;
        public int Owner;
        public bool Connectable;
        public Pod(GlobalThermoGame game)
            : base(game)
        {
            Connectable = true;
        }
    }
}
