using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Animation
{
    public class SpriteAnimation : Sprite
    {
        int fps;
        double timePerFrame;
        private double timeSinceLastStep = 0;
        private int frameCount;
        private int currentFrame;
        private Rectangle[] sheets;

        public void initialize(ContentManager cm, string name, Rectangle[] sheets, int fps, int frameCount)
        {
            texture = cm.Load<Texture2D>(name);
            this.fps = fps;
            timePerFrame = 1 / fps;
            this.frameCount = frameCount;
            this.sheets = sheets;
        }

        public void nextFrame()
        {
            currentFrame = (currentFrame + 1) % frameCount;
            sourceRectangle = sheets[currentFrame];
        }

        public override void update(GameTime gameTime) 
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;

            timeSinceLastStep += time;

            if(timeSinceLastStep > timePerFrame)
            {
                nextFrame();
                timeSinceLastStep = timeSinceLastStep - timePerFrame;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, _scale, effects, layerDepth);
        }
    }
}
