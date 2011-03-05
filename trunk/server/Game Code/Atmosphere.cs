using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalThermo.Pods;

namespace GlobalThermo
{
    public class Atmosphere : IGameEntity
    {
        public double InnerRadius;
        public double OuterRadius;
        public ResourceType ResourceType;
        public PodType ResourcePodType;

        public Atmosphere(double innerRadius, double outerRadius, ResourceType resourceType, PodType resourcePodType)
        {
            this.InnerRadius = innerRadius;
            this.OuterRadius = outerRadius;
            this.ResourceType = resourceType;
            this.ResourcePodType = resourcePodType;
        }

        public bool IsWithin(Vector2D pt)
        {
            return pt.MagnitudeSquared() >= InnerRadius * InnerRadius
                && pt.MagnitudeSquared() < OuterRadius * OuterRadius;
        }

        public void Simulate(double timeDelta)
        {

        }
    }
}
