using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public class Atmosphere : IGameEntity
    {
        public double innerRadius;
        public double outerRadius;
        public ResourceType resourceType;

        public Atmosphere(double innerRadius, double outerRadius, ResourceType resourceType)
        {
            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.resourceType = resourceType;
        }

        public bool IsWithin(Vector2D pt)
        {
            return pt.MagnitudeSquared() >= innerRadius * innerRadius
                && pt.MagnitudeSquared() <  outerRadius * outerRadius;
        }

        public void Simulate(double timeDelta)
        {

        }
    }
}
