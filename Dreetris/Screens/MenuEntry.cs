using Dreetris.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public float Width { get { return _width; } }
        public float Height { get { return _height; } }
        public virtual Vector2 Position { get { return _position; } set { _position = value; } }

        public Vector2 Size { get { return _size; } }

        public MenuEntry(GameObjects gameObjects)
        {
            this.assetManager = gameObjects.AssetManager;
            font = assetManager.GetFont("Font");

            _position = new Vector2(0, 0);
        }

        public abstract void call();
        public abstract void update(GameTime gameTime);
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
