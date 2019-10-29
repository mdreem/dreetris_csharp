using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Screens
{
    public class MenuEntryText : MenuEntry
    {
        string entry;
        Action callFunction;

        Vector2 shadowPosition;

        float shadowX = 2f;
        float shadowY = 2f;

        public override Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                shadowPosition = new Vector2(_position.X + shadowX, _position.Y + shadowY);
            }
        }

        public MenuEntryText(GameObjects gameObjects, string entry, Action call)
            : base(gameObjects)
        {
            this.entry = entry;
            this.callFunction = call;

            _size = font.MeasureString(entry);
            _size.X += shadowX;
            _size.Y += shadowY;
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
        }

        public override void update(GameTime gameTime)
        {

        }
    }
}
