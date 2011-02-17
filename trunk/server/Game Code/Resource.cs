using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
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
        public ResourceType Type;

        public Resource(ResourceType type, double quantity)
        {
            Type = type;
            Quantity = quantity;
        }
    }
}
