using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Screens
{
    public class FloatSlider
    {
        AssetManager assetManager;

        float min;
        float max;
        int steps;

        float width;
        float height;

        Vector2 _position;

        Sprite slider;
        protected SpriteFont font;

        int sliderPosition;

        Vector2 _size;

        public Vector2 size { get { return _size; } }

        public Vector2 position
        {
            get { return _position; }
            set
            {
                _position = value;
                SetPosition();
            }
        }

        public FloatSlider(GameObjects gameObjects, float min, float max, int steps, float width, int sliderPosition)
        {
            this.assetManager = gameObjects.AssetManager;

            this.min = min;
            this.max = max;
            this.steps = steps;
            this.sliderPosition = sliderPosition;
            this.width = width;

            position = new Vector2(100, 450);

            slider = assetManager.GetSprite("slider");
            slider.CenterCoordinates();

            height = slider.height;

            SetPosition();

            font = assetManager.GetFont("Font");

            _size.X = width + slider.width;
            _size.Y = slider.height;
        }

        public FloatSlider(GameObjects gameObjects, float min, float max, int steps, float width)
            : this(gameObjects, min, max, steps, width, 0)
        {
        }

        private void SetPosition()
        {
            //TODO: Include height of line
            if (slider != null)
            {
                slider.position.X = position.X;
                slider.position.Y = position.Y + height / 2;
            }
        }

        public void MoveLeft()
        {
            if (sliderPosition != 0)
                sliderPosition--;
        }

        public void MoveRight()
        {
            if (sliderPosition < steps)
                sliderPosition++;
        }

        public float Value()
        {
            return MathHelper.Clamp(min + sliderPosition * (max - min) / (steps - 1), min, max);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 start = new Vector2(position.X, position.Y + height / 2);
            Vector2 end = new Vector2(position.X + width, position.Y + height / 2);

            Drawing.DrawLine(spriteBatch, 2, Color.Wheat, start, end);

            slider.SetTransparency(0.5f);
            slider.position.X = position.X + width * sliderPosition / (steps - 1);
            slider.Draw(spriteBatch);

#if DEBUG
            spriteBatch.DrawString(font, Value().ToString(), new Vector2(position.X + width + 20, position.Y), Color.White);
#endif
        }
    }
}
