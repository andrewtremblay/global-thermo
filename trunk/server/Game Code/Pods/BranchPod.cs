using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo.Pods
{
    public class BranchPod : Pod
    {
        protected int connections;

        public BranchPod(Player owner, int podId, Vector2D position)
            : base(owner, podId, position)
        {
            connections = 2;
        }

        public override void Connect()
        {
            connections -= 1;
        }

        public override bool IsConnectable()
        {
            return connections > 0;
        }

        public override void Simulate(double timeDelta)
        {

        }
    }
}
