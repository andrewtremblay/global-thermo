using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace global_thermo.Game.Particles
{
    class ParticleGroup : GameObjectGroup
    {
        private int nParticles;

        public ParticleGroup(GlobalThermoGame game, Type particleType, int nParticles)
            : base(game)
        {
            this.nParticles = nParticles;
            for (int i = 0; i < nParticles; i++)
            {
                ConstructorInfo[] ci = particleType.GetConstructors();
                // We pass the type of the particle in, and so now we have to invoke its constructor.
                ParticleSprite ps =
                    (ParticleSprite)ci[0].Invoke(new object[] { game });
                Children.Add(ps);
            }
        }

        public void AddParticle(Vector2 pos, Vector2 velocity, float rotation, float rotationalVelocity)
        {
            // Find the first invisible particle and respawn it
            ParticleSprite chosen = (ParticleSprite)Children[0];
            foreach (ParticleSprite ps in Children)
            {
                if (!ps.Visible)
                {
                    chosen = ps;
                    break;
                }
            }

            chosen.Spawn(pos, velocity, rotation, rotationalVelocity);
        }
    }
}
