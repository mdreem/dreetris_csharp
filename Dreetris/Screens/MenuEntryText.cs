using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    public class MenuEntryText : MenuEntry
    {
        string entry;
        Menu.Call callFunction;

        Vector2 shadowPosition;

        float shadowX = 2f;
        float shadowY = 2f;

        public override Vector2 position
        {
            get { return _position; }
            set
            {
                _position = value;
                shadowPosition = new Vector2(_position.X + shadowX, _position.Y + shadowY);
            }
        }

        public MenuEntryText(Game game, AssetManager assetManager, string entry, Menu.Call call)
            : base(game, assetManager)
        {
            this.entry = entry;
            this.callFunction = call;

            _size = font.MeasureString(entry);
            _size.X += shadowX;
            _size.Y += shadowY;

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
        }

        public override void update(GameTime gameTime)
        {

        }
    }
}
