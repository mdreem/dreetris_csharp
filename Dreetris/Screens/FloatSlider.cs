using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        Vector2 position;

        Sprite slider;
        protected SpriteFont font;

        int sliderPosition;

        public FloatSlider(AssetManager assetManager, float min, float max, int steps, int sliderPosition)
        {
            this.assetManager = assetManager;

            this.min = min;
            this.max = max;
            this.steps = steps;
            this.sliderPosition = sliderPosition;

            position = new Vector2(100, 450);
            width = 200;
            height = 10;

            slider = assetManager.getSprite("slider");
            slider.centerCoordinates();

            slider.position.X = position.X;
            slider.position.Y = position.Y + height / 2;

            font = assetManager.getFont("Font");
        }

        public FloatSlider(AssetManager assetManager, float min, float max, int steps)
            : this(assetManager, min, max, steps, 0)
        {
        }

        public void moveLeft()
        {
            if (sliderPosition != 0)
                sliderPosition--;
        }

        public void moveRight()
        {
            if (sliderPosition < steps)
                sliderPosition++;
        }

        public float value()
        {
            return MathHelper.Clamp(min + sliderPosition * (max - min) / (steps - 1), min, max);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 start = new Vector2(position.X, position.Y + height / 2);
            Vector2 end = new Vector2(position.X + width, position.Y + height / 2);

            Drawing.DrawLine(spriteBatch, 2, Color.Wheat, start, end);

            slider.position.X = position.X + width * sliderPosition / (steps - 1);
            slider.draw(spriteBatch);

            spriteBatch.DrawString(font, value().ToString(), new Vector2(position.X + width + 20, position.Y), Color.White);
        }
    }
}
