using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Particles
{
    public class Particle
    {
        //Vector2 position;
        protected Vector2 velocity;
        protected Vector2 acceleration;

        protected float lifetime;
        protected float timeToLive;

        protected float ttlPercentage;

        protected Sprite sprite;
        protected ParticleSystem particleSystem;
        private bool _endOfLife;

        public Particle(Sprite sprite, Vector2 velocity, Vector2 acceleration, float timeToLive, ParticleSystem particleSystem)
        {
            this.sprite = sprite; //.Clone();
            //sprite.position; = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.timeToLive = timeToLive;
            this.lifetime = 0;
            this.ttlPercentage = 0;
            this.particleSystem = particleSystem;
        }

        public virtual void update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            lifetime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (lifetime > timeToLive)
            {
                lifetime = timeToLive;
                _endOfLife = true;
            }
            ttlPercentage = lifetime / timeToLive;

            sprite.setTransparency(1.0f - ttlPercentage);

            sprite.position = acceleration * t * t * 0.5f + velocity * t + sprite.position;
            velocity = acceleration * t + velocity;
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            sprite.draw(spriteBatch);
        }

        public bool endOfLife { get { return _endOfLife; } }
    }
}
