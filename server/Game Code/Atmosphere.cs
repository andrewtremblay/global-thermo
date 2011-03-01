using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{
    public class Atmosphere : IGameEntity
    {
<<<<<<< .mine
        public double InnerRadius;
        public double OuterRadius;
        public ResourceType ResourceType;
        public int NumCollecting;
=======
        public double InnerRadius;
        public double OuterRadius;
        public ResourceType ResourceType;
>>>>>>> .r35

        public Atmosphere(double innerRadius, double outerRadius, ResourceType resourceType)
        {
<<<<<<< .mine
            this.InnerRadius = innerRadius;
            this.OuterRadius = outerRadius;
            this.ResourceType = resourceType;
            NumCollecting = 0;
=======
            this.InnerRadius = innerRadius;
            this.OuterRadius = outerRadius;
            this.ResourceType = resourceType;
>>>>>>> .r35
        }

        public bool IsWithin(Vector2D pt)
        {
<<<<<<< .mine
            return pt.MagnitudeSquared() >= InnerRadius * InnerRadius
                && pt.MagnitudeSquared() <  OuterRadius * OuterRadius;
=======
            return pt.MagnitudeSquared() >= InnerRadius * InnerRadius
                && pt.MagnitudeSquared() < OuterRadius * OuterRadius;
>>>>>>> .r35
        }

        public void Simulate(double timeDelta)
        {
            // NumCollecting 
        }
    }
}
