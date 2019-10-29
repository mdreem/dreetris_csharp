using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Particles
{
    public class ParticleSystem
    {
        protected List<Particle> particles = new List<Particle>();

        public virtual void update(GameTime gameTime)
        {
            foreach (var particle in particles)
            {
                particle.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in particles)
            {
                particle.draw(spriteBatch);
            }
        }

        public void Remove(Particle particle)
        {
            particles.Remove(particle);
        }
    }
}
