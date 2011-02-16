using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    class ResourcePod : Pod
    {
        public double Rate;

        public ResourcePod(Player owner, int podId, Vector2D position)
            : base(owner, podId, position)
        {
            Rate = 1.0;
        }

        public override void Simulate(double timeDelta)
        {
            
        }

        private double resourceTickTimer;
    }
}
