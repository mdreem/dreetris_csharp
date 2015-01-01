using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    class MenuEntrySlider : MenuEntry
    {
        string entry;
        Action callFunction;

        Vector2 shadowPosition;
        Vector2 textSize;

        float shadowX = 2f;
        float shadowY = 2f;

        float padding;

        protected FloatSlider slider;

        public override Vector2 position
        {
            get { return _position; }
            set
            {
                _position = value;
                setPositions();
            }
        }

        public MenuEntrySlider(GameObjects gameObjects, string entry, Action call)
            : base(gameObjects)
        {
            this.entry = entry;
            this.callFunction = call;

            slider = new FloatSlider(gameObjects, 1.0f, 5.0f, 100, 200);

            padding = 20;

            setSize();
            setPositions();
        }

        private void setSize()
        {
            textSize = font.MeasureString(entry);
            Vector2 sliderSize = slider.size;

            _size = new Vector2(textSize.X + shadowX + sliderSize.X + padding, Math.Max(sliderSize.Y, textSize.Y + shadowY));
        }

        private void setPositions()
        {
            Vector2 newSliderPosition = new Vector2(
                        _position.X + textSize.X + padding,
                        _position.Y + slider.size.Y / 2);
            slider.position = newSliderPosition;
            //slider.position.Y += slider.size.Y / 2;

            shadowPosition = new Vector2(_position.X + shadowX, _position.Y + shadowY);
        }

        public override void call()
        {
            callFunction.Invoke();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Color col = new Color(180, 160, 190);
            spriteBatch.DrawString(font, entry, shadowPosition, Color.Black); //Shadow
            spriteBatch.DrawString(font, entry, _position, col);

            slider.draw(spriteBatch);
        }

        public override void update(GameTime gameTime)
        {

        }

        public void moveLeft()
        {
            slider.moveLeft();
        }

        public void moveRight()
        {
            slider.moveRight();
        }
    }
}
