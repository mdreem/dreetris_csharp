using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Dreetris.Particles
{
    public class ParticleSystem
    {
        protected List<Particle> particles = new List<Particle>();

        public virtual void update(GameTime gameTime)
        {
            foreach (var particle in particles)
            {
                particle.update(gameTime);
            }
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            foreach(var particle in particles)
            {
                particle.draw(spriteBatch);
            }
        }

        public void remove(Particle particle)
        {
            particles.Remove(particle);
        }
    }
}
