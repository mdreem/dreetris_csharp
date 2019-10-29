using System;
using Dreetris.Animation;
using Microsoft.Xna.Framework;

namespace Dreetris.Particles
{
    public class DissolvingSprite : ParticleSystem
    {
        private Random random;

        // TODO: slicing wrong if not divisible
        // TODO: general acceleration
        public DissolvingSprite(Sprite sprite, float timeToLive = 0.0f, float timeToLiveDelta = 0.0f, float Xdelta = 0.0f, float Ydelta = 0.0f, int sizeX = 1, int sizeY = 1)
        {
            Rectangle bounds = sprite.GetSourceRectangle();
            int width = bounds.Width / sizeX;
            int height = bounds.Height / sizeY;

            random = SingleRandom.random;

            for (int i = 0; i <= width; i++)
            {
                for (int j = 0; j <= height; j++)
                {
                    AddSpritePart(sprite, timeToLive, timeToLiveDelta, Xdelta, Ydelta, bounds, i, j, sizeX, sizeY);
                }
            }
        }

        private void AddSpritePart(Sprite sprite, float timeToLive, float timeToLiveDelta, float Xdelta, float Ydelta, Rectangle bounds, int i, int j, int sizeX, int sizeY)
        {
            int tileWidth = sizeX;
            int tileHeight = sizeY;

            if (sizeX * i > bounds.Width)
            {
                tileWidth = bounds.Width % sizeX;
                if (tileWidth == 0)
                    return;
            }

            if (sizeY * j > bounds.Height)
            {
                tileHeight = bounds.Height % sizeY;
                if (tileHeight == 0)
                    return;
            }

            Sprite newSprite = sprite.Clone();
            newSprite.CenterCoordinates();
            //cut out part of the sprite
            Rectangle newSpriteSection = new Rectangle(bounds.X + sizeX * i, bounds.Y + sizeY * j, tileWidth, tileHeight);
            newSprite.SetSourceRectangle(newSpriteSection);
            //put the new sprite so that it will overlap with the original positon
            newSprite.position.X += sizeX * i;
            newSprite.position.Y += sizeY * j;

            Vector2 velocity = new Vector2(
                            Xdelta * (float)(random.NextDouble() * 2 - 1),
                            Ydelta * (float)(random.NextDouble() * 2 - 1));

            Particle particle = new Particle(newSprite, velocity, new Vector2(0, 30.0f),
                                        0.7f * (float)(random.NextDouble() * 2 - 1),
                                        timeToLive + timeToLiveDelta * (float)(random.NextDouble() * 2 - 1),
                                        this);

            particles.Add(particle);
        }

        public override void update(GameTime gameTime)
        {
            particles.RemoveAll(x => x.endOfLife);

            base.update(gameTime);
        }
    }
}
