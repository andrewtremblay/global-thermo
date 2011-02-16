using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalThermo
{

    public enum PodType
    {
        Residence,
        Mining,
        Defense,
        Branch
    }

    public class PodFactory
    {
        public PodFactory()
        {

        }

        public bool CreatePod(PodType type, Player player, Vector2D location)
        {
            return false;
        }
    }
}
