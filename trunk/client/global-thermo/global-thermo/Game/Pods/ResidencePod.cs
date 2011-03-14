﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace global_thermo.Game.Pods
{
    public class ResidencePod : ResourcePod
    {
        public ResidencePod(GlobalThermoGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            LoadTexture(game.Content.Load<Texture2D>("images/gameplay/pod_residence"));
        }
    }
}
