using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    public abstract class MenuEntry
    {
        protected AssetManager assetManager;
        protected SpriteFont font;

        protected Vector2 _size;

        public Vector2 _position;
        protected float _width;
        protected float _height;

        public float width { get { return _width; } }
        public float height { get { return _height; } }
        public virtual Vector2 position { get { return _position; } set { _position = value; } }

        public Vector2 size { get { return _size; } }

        public MenuEntry(GameObjects gameObjects)

        {
            this.assetManager = gameObjects.assetManager;
            font = assetManager.getFont("Font");

            _position = new Vector2(0, 0);
        }

        public abstract void call();
        public abstract void update(GameTime gameTime);
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
