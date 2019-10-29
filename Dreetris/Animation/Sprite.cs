using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Animation
{
    public class Sprite
    {
        #region fields and properties

        protected Texture2D texture;
        public Vector2 position = new Vector2();
        protected Nullable<Rectangle> sourceRectangle;
        protected Color color = Color.White;
        protected Color originalColor = Color.White;
        public float rotation;
        protected Vector2 origin = new Vector2(0, 0);
        protected Vector2 _scale = new Vector2(1, 1);
        protected Vector2 scale_original = new Vector2(1, 1);

        protected SpriteEffects effects = SpriteEffects.None;
        protected float layerDepth;

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

        public Rectangle GetSourceRectangle()
        {
            if (sourceRectangle != null)
                return (Rectangle)sourceRectangle;
            else
                return texture.Bounds;
        }

        public void SetSourceRectangle(Rectangle? sourceRectangle)
        {
            this.sourceRectangle = sourceRectangle;
        }

        public void SetTransparency(float transparency = 1.0f)
        {
            this.transparency = transparency;
            color = originalColor * transparency;
        }

        public virtual void Initialize(ContentManager cm, string name)
        {
            texture = cm.Load<Texture2D>(name);
            _height = texture.Bounds.Height;
            _width = texture.Bounds.Width;
        }

        public virtual void Scale(float scalefactor = 1.0f)
        {
            _scale = scale_original * scalefactor;
            _height = (int)(texture.Bounds.Height * scalefactor);
            _width = (int)(texture.Bounds.Width * scalefactor);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, _scale, effects, layerDepth);
        }

        public virtual Sprite Clone()
        {
            return new Sprite(this);
        }

        public void CenterCoordinates()
        {
            if (!sourceRectangle.HasValue)
            {
                origin.X = (float)texture.Width / 2;
                origin.Y = (float)texture.Height / 2;
            }
            else
            {
                origin.X = (float)sourceRectangle.Value.Width / 2;
                origin.Y = (float)sourceRectangle.Value.Height / 2;
            }
        }

        #endregion
    }
}
