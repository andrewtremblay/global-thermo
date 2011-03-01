using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace global_thermo.Game
{
    public enum ResourceType
    {
        Ground,
        Atmo1,
        Atmo2,
        Atmo3
    }

    public class Resource
    {
        public double Quantity;
        public ResourceType RType;

        public Resource(ResourceType type)
        {
            RType = type;
            Quantity = 0;
        }
    }
}
