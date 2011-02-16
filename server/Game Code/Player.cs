using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIO.GameLibrary;

namespace GlobalThermo
{
    public class Player : BasePlayer, IGameEntity
    {
        public String Name;
        public UInt32 Color;
        public List<Pod> Pods;
        public List<Resource> Resources;

        public void Simulate(double timeDelta)
        {
            foreach (Pod pod in Pods)
            {
                pod.Simulate(timeDelta);
            }
        }
    }
}
