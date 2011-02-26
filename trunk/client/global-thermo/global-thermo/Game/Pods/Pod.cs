using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace global_thermo.Game.Pods
{
    public enum PodType
    {
        Residence,
        Resource,
        Defense,
        Branch
    }

    public abstract class Pod : Sprite
    {
        public int PodID;
        public int Owner;
        public Pod(GlobalThermoGame game)
            : base(game)
        {

        }
    }
}
