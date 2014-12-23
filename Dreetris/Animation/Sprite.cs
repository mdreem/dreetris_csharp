using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Dreetris.Animation
{
    /// <summary>
    /// Encapsulates all the properties used by SpriteBatch.Draw 
    /// </summary>
    public class Sprite
    {
        #region fields and properties

        protected Texture2D texture;
        public Vector2 position;
        protected Nullable<Rectangle> sourceRectangle = null;
        protected Color color = Color.White;
        protected Color originalColor = Color.White;
        protected float rotation = 0;
        protected Vector2 origin = new Vector2(0, 0);
        protected Vector2 _scale = new Vector2(1, 1);
        protected Vector2 scale_original = new Vector2(1, 1);

        protected SpriteEffects effects = SpriteEffects.None;
        protected float layerDepth = 0;

        protected int _width;
        protected int _height;
        private float transparency = 1.0f;

        public int width { get { return _width; } }
        public int height { get { return _height; } }

        #endregion

        #region constructors

        public Sprite() { }

        public Sprite(Sprite copySprite)
        {
            texture = copySprite.texture;
            position = copySprite.position;
            sourceRectangle = copySprite.sourceRectangle;
            color = copySprite.color;
            originalColor = copySprite.originalColor;
            rotation = copySprite.rotation;
            origin = copySprite.origin;
            _scale = copySprite._scale;
            scale_original = copySprite.scale_original;

            effects = copySprite.effects;
            layerDepth = copySprite.layerDepth;

            _width = copySprite._width;
            _height = copySprite._height;
            transparency = copySprite.transparency;
        }

        #endregion

        #region public methods

        public Rectangle getSourceRectangle()
        {
            if (sourceRectangle != null)
                return (Rectangle) sourceRectangle;
            else
                return texture.Bounds;
        }

        public void setSourceRectangle(Rectangle? sourceRectangle)
        {
            this.sourceRectangle = sourceRectangle;
        }

        public void setTransparency(float transparency = 1.0f)
        {
            this.transparency = transparency;
            color = originalColor * transparency;
        }

        public virtual void initialize(ContentManager cm, string name)
        {
            texture = cm.Load<Texture2D>(name);
            _height = texture.Bounds.Height;
            _width = texture.Bounds.Width;
        }

        public virtual void scale(float scalefactor = 1.0f)
        {
            _scale = scale_original * scalefactor;
            _height = (int)(texture.Bounds.Height * scalefactor);
            _width = (int)(texture.Bounds.Width * scalefactor);
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

        #endregion
    }
}
