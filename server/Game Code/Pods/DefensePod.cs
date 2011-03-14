using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    public class DefensePod : Pod
    {
        public DefensePod(Player owner, int podId, Vector2D position)
            : base(owner, podId, position)
        {

        }

        public override void Simulate(double timeDelta)
        {
            
        }
    }
}
