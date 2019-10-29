using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Screens
{
    public class FadeScreen : Screen
    {
        public enum Type
        {
            FADE_IN,
            FADE_OUT,
            FADE_OUT_POP
        }

        Type transitionType;

        Rectangle backgroundRectangle;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        private double fadeDuration = 1500;

        private Texture2D blank;
        private double transitionDuration;
        private float transitionPercentange;

        SpriteBatch spriteBatch;
        private float maxFade;

        public FadeScreen(GameObjects gameObjects,
                Type transitionType,
                float maxFade = 1.0f,
                float fadeDuration = 1000)
            : base(gameObjects)
        {
            this.maxFade = maxFade;
            this.transitionType = transitionType;
            this.fadeDuration = fadeDuration;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            backgroundRectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            double time = gameTime.ElapsedGameTime.TotalMilliseconds;
            transitionDuration += time;

            if (transitionDuration > fadeDuration)
            {
                transitionPercentange = 1.0f;
                screenManager.Pop();

                if (transitionType == Type.FADE_OUT_POP)
                    screenManager.Pop();
            }
            else
            {
                transitionPercentange = (float)(transitionDuration / fadeDuration);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Color overlayCol;

            spriteBatch.Begin();
            switch (transitionType)
            {
                case Type.FADE_IN:
                    overlayCol = new Color(new Vector4(0, 0, 0, (1.0f - transitionPercentange) * maxFade));
                    spriteBatch.Draw(blank, backgroundRectangle, overlayCol);
                    break;
                case Type.FADE_OUT:
                case Type.FADE_OUT_POP:
                    overlayCol = new Color(new Vector4(0, 0, 0, transitionPercentange * maxFade));
                    spriteBatch.Draw(blank, backgroundRectangle, overlayCol);
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
