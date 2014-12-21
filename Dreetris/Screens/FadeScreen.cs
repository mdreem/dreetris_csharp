using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Screens
{
    /// <summary>
    /// This class is used for transitioning between two Screens via a simple fade in or fade out
    /// </summary>
    public class FadeScreen : Screen
    {
        public enum Type
        {
            FADE_IN,
            FADE_OUT
        }

        Type transitionType;

        Rectangle backgroundRectangle;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600; //TODO: Übergeben!

        private double fadeDuration = 1500;

        private Texture2D blank;
        private double transitionDuration;
        private float transitionPercentange;

        SpriteBatch spriteBatch;
        private float maxFade;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="screenManager"></param>
        /// <param name="transitionType"></param>
        /// <param name="maxFade">maximum transparency value for the fade</param>
        /// <param name="fadeDuration">duration of the transition in milliseconds</param>
        public FadeScreen(Game game,
                ScreenManager screenManager,
                Type transitionType,
                float maxFade = 1.0f,
                float fadeDuration = 1000)
            : base(game, screenManager)
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
                screenManager.pop();
            }
            else
            {
                // compute the portion of the transition that already has passed as a percentage.
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
