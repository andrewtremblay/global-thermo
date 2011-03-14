using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Particles
{
    public class ParticleSprite : Sprite
    {
        public Vector2 Velocity;
        public double AngularVelocity;

        public ParticleSprite(GlobalThermoGame game)
            : base(game)
        {
            Visible = false;
            Active = false;
        }

        protected override void updateSelf(double deltaTime)
        {
            RectPosition += new Vector2(Velocity.X * (float)deltaTime, Velocity.Y * (float)deltaTime);
            Angle += AngularVelocity * deltaTime;
        }

        public virtual void Spawn(Vector2 position, Vector2 velocity, double angle, double angularVelocity)
        {
            Visible = true;
            Active = true;
            RectPosition = position;
            this.Angle = angle;
            this.AngularVelocity = angularVelocity;
            this.Velocity = velocity;
        }
    }
}
