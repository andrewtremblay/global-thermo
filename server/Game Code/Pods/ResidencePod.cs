using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    class ResidencePod : Pod
    {
        public int People;
        public ResidencePod(Player owner, int podId, Vector2D position, int people)
            : base(owner, podId, position)
        {
            People = people;
            Type = PodType.Residence;
        }

        public override void Simulate(double timeDelta)
        {

        }
    }
}
