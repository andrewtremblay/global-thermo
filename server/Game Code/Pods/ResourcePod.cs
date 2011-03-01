using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    class ResourcePod : Pod
    {
        public double Rate;
        public ResourceType RType;

        public ResourcePod(Player owner, int podId, Vector2D position, ResourceType resourceType, double resourceCollectAmt)
            : base(owner, podId, position)
        {
            Rate = 1.0;

            this.RType = resourceType;
            this.resourceCollectAmt = resourceCollectAmt;
            this.resourceTimer = 0;
            Type = PodType.Resource;
        }

        public override void Simulate(double timeDelta)
        {
            // If rate is positive, it means we're collecting resources for the player
            if (Rate > 0)
            {
                resourceTimer += timeDelta * Rate;
                // If you've gained one resource, the player has earned the collectAmt of that resource
                if (resourceTimer > 1.0)
                {
                    resourceTimer -= 1.0;
                    this.owner.getResourceByType(RType).Quantity += resourceCollectAmt;
                }

                // We need to also do something about the lava, and about depleting the atmo levels
            }
            else
            {
                // Some sort of recycling behavior
            }
        }

        public double GetCollectionRate()
        {
            return Rate * resourceCollectAmt;
        }

        private double resourceTimer;
        private double resourceCollectAmt;
        
    }
}
