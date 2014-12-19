using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Animation
{
    public class Sprite
    {
        protected Texture2D texture;
        public Vector2 position;
        protected Nullable<Rectangle> sourceRectangle = null;
        protected Color color = Color.White;
        protected float rotation = 0;
        protected Vector2 origin = new Vector2(0, 0);
        protected Vector2 _scale = new Vector2(1, 1);
        protected Vector2 scale_original = new Vector2(1, 1);

        protected SpriteEffects effects = SpriteEffects.None;
        protected float layerDepth = 0;

        protected int _width;
        protected int _height;

        public int width { get { return _width; } }
        public int height { get { return _height; } }

        public Sprite() { }

        public Sprite(Sprite copySprite)
        {
            texture = copySprite.texture;
            position = copySprite.position;
            sourceRectangle = copySprite.sourceRectangle;
            color = copySprite.color;
            rotation = copySprite.rotation;
            origin = copySprite.origin;
            _scale = copySprite._scale;
            scale_original = copySprite.scale_original;

            effects = copySprite.effects;
            layerDepth = copySprite.layerDepth;

            _width = copySprite._width;
            _height = copySprite._height;
        }

        public virtual void initialize(ContentManager cm, string name)
        {
            texture = cm.Load<Texture2D>(name);
            _height = texture.Bounds.Height;
            _width = texture.Bounds.Width;
        }

        public void scale(float scalefactor = 1.0f)
        {
            _scale = scale_original * scalefactor;
        }

        public virtual void update(GameTime gameTime) { }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, _scale, effects, layerDepth);
        }

        public virtual Sprite Clone()
        {
            return new Sprite(this);
        }
    }
}
