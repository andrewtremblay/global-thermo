using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace global_thermo.Game
{
    public class GameObjectGroup : GameObject
    {
        public GameObjectGroup(GlobalThermoGame game)
            : base(game)
        {

        }

        public void Disable() { Visible = false; Active = false; }
        public void Enable() { Visible = true; Active = true; }
    }
}
