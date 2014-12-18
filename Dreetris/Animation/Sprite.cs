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
        Texture2D texture;
        public Vector2 position;
        Nullable<Rectangle> sourceRectangle = null;
        Color color = Color.White;
        float rotation = 0;
        Vector2 origin = new Vector2(0, 0);
        Vector2 scale = new Vector2(1, 1);
        SpriteEffects effects = SpriteEffects.None;
        float layerDepth = 0;

        int _width;
        int _height;

        public int width { get { return _width; } }
        public int height { get { return _height; } }

        public virtual void initialize(ContentManager cm, string name)
        {
            texture = cm.Load<Texture2D>(name);
            _height = texture.Bounds.Height;
            _width = texture.Bounds.Width;
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public Sprite Clone()
        {
            return (Sprite)this.MemberwiseClone();
        }
    }
}
