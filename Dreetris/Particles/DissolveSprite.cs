using Dreetris.Animation;
using Microsoft.Xna.Framework;
using System;

namespace Dreetris.Particles
{
    public class DissolveSprite : ParticleSystem
    {
        private Random random;

        public DissolveSprite(Sprite sprite)
            : base()
        {
            Rectangle bounds = sprite.getSourceRectangle();
            int width = bounds.Width;
            int height = bounds.Height;

            random = new Random();

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    Sprite newSprite = sprite.Clone();
                    Rectangle newSpriteSection = new Rectangle(bounds.X + i, bounds.Y + j, 1, 1);
                    newSprite.setSourceRectangle(newSpriteSection);
                    newSprite.position.X = newSprite.position.X + i;
                    newSprite.position.Y = newSprite.position.Y + j;

                    Vector2 velocity = new Vector2(
                                    10f * (float)(random.NextDouble() * 2 - 1),
                                    10f * (float)(random.NextDouble() * 2 - 1));

                    Particle particle = new Particle(newSprite, velocity, new Vector2(0, 6.0f), 2000, this);

                    particles.Add(particle);
                }
        }

        public override void update(GameTime gameTime)
        {
            particles.RemoveAll(x => x.endOfLife);

            base.update(gameTime);
        }
    }
}
