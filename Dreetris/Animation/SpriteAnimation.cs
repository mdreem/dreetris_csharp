using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Animation
{
    public class SpriteAnimation : Sprite
    {
        #region fields and properties

        int fps;
        double timePerFrame;
        private double timeSinceLastStep;
        private int frameCount;
        private int currentFrame;
        private Rectangle[] sheets;

        #endregion

        #region constructors

        public SpriteAnimation() { }

        public SpriteAnimation(SpriteAnimation copySpriteAnimation)
            : base(copySpriteAnimation)
        {
            fps = copySpriteAnimation.fps;
            timePerFrame = copySpriteAnimation.timePerFrame;
            timeSinceLastStep = copySpriteAnimation.timeSinceLastStep;
            frameCount = copySpriteAnimation.frameCount;
            currentFrame = copySpriteAnimation.currentFrame;
            sheets = copySpriteAnimation.sheets;
        }

        #endregion

        #region public methods

        public void Initialize(ContentManager cm, string name, Rectangle[] sheets, int fps, int frameCount)
        {
            texture = cm.Load<Texture2D>(name);
            this.fps = fps;
            timePerFrame = 1000 / (float)fps;
            this.frameCount = frameCount;
            this.sheets = sheets;

            SetSourceRectangle(0);

            Console.WriteLine("Animation.FPS: {0} -> {1}", fps, timePerFrame);
        }

        public void NextFrame()
        {
            currentFrame = (currentFrame + 1) % frameCount;
            SetSourceRectangle(currentFrame);
        }

        private void SetSourceRectangle(int frame)
        {
            sourceRectangle = sheets[frame];

            _width = sheets[frame].Width;
            _height = sheets[frame].Height;
        }

        public override void Scale(float scalefactor = 1.0f)
        {
            _scale = scale_original * scalefactor;
            _height = (int)(sheets[currentFrame].Height * scalefactor);
            _width = (int)(sheets[currentFrame].Width * scalefactor);
        }

        public override void Update(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;

            timeSinceLastStep += time;

            if (timeSinceLastStep > timePerFrame)
            {
                NextFrame();
                timeSinceLastStep = timeSinceLastStep - timePerFrame;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, _scale, effects, layerDepth);
        }

        public override Sprite Clone()
        {
            return new SpriteAnimation(this);
        }

        #endregion
    }
}
