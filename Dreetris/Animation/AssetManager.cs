using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dreetris.Animation
{
    public class AssetManager
    {
        ContentManager contentManager;
        private XDocument config;
        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public AssetManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            config = XDocument.Load("Content/Sprites.xml");
        }

        public Sprite GetSprite(string name)
        {
            if (sprites.ContainsKey(name))
            {
                return sprites[name].Clone();
            }
            else
            {
                // reads the data inside a <sprite> </sprite>-block containing the correct <name>-block
                var data = (from sprite_data in config.Descendants("Sprite")
                            where sprite_data.Element("name").Value == name
                            select sprite_data).First();

                var type = data.Element("type");

                Sprite sprite = null;

                if (type != null)
                {
                    if (type.Value.ToLower().Equals("animation"))
                        sprite = GetSpriteAnimationFromData(data);
                }
                else
                {
                    sprite = GetSpriteFromData(data);
                }
                sprites[name] = sprite;

                return sprite.Clone();
            }
        }

        protected Sprite GetSpriteFromData(XElement data)
        {
            Sprite sprite = new Sprite();
            string textureName = data.Element("texture").Value;
            sprite.Initialize(contentManager, textureName);
            return sprite;
        }

        protected SpriteAnimation GetSpriteAnimationFromData(XElement data)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            string textureName = data.Element("texture").Value; ;
            int fps = int.Parse(data.Element("fps").Value);

            foreach (var frame in data.Elements("frame"))
            {
                int x = int.Parse(frame.Element("x").Value);
                int y = int.Parse(frame.Element("y").Value);
                int width = int.Parse(frame.Element("width").Value);
                int height = int.Parse(frame.Element("height").Value);

                rectangles.Add(new Rectangle(x, y, width, height));
            }

            SpriteAnimation spriteAnimation = new SpriteAnimation();
            spriteAnimation.Initialize(contentManager, textureName, rectangles.ToArray(), fps, rectangles.Count);

            return spriteAnimation;
        }

        public SoundEffect GetSoundEffect(string name)
        {
            return contentManager.Load<SoundEffect>(name);
        }

        public SpriteFont GetFont(string name)
        {
            return contentManager.Load<SpriteFont>(name);
        }
    }
}
