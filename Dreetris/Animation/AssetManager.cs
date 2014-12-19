using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
//using System.Collections.Generic; 

namespace Dreetris.Animation
{
    public class AssetManager
    {
        ContentManager contentManager;
        Game1 game;
        private XDocument config;

        public AssetManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            config = XDocument.Load("Sprites.xml");
        }

        public Sprite getSprite(string name)
        {
            Console.WriteLine(string.Format("Getting sprite data for: {0}", name));
   
            // reads the data inside a <sprite> </sprite>-block containing the correct <name>-block
            var data = (from sprite_data in config.Descendants("Sprite")
                          where sprite_data.Element("name").Value == name
                          select sprite_data).First();

            Console.WriteLine(data.ToString());

            var type = data.Element("type");

            Sprite sprite = null;

            if (type != null)
            {
                if (type.Value.ToLower().Equals("animation"))
                    sprite = getSpriteAnimationFromData(data);
            }
            else
            {
                sprite = getSpriteFromData(data);
            }
            return sprite;
        }

        protected Sprite getSpriteFromData(XElement data)
        {
            Sprite sprite = new Sprite();
            string textureName = data.Element("texture").Value;
            sprite.initialize(contentManager, textureName);
            return sprite;
        }

        protected SpriteAnimation getSpriteAnimationFromData(XElement data)
        {
            Console.WriteLine("Build animation: ");
            List<Rectangle> rectangles = new List<Rectangle>();
            string textureName = data.Element("texture").Value;;
            int fps = int.Parse(data.Element("fps").Value);

            foreach (var frame in data.Elements("frame"))
            {
                int x = int.Parse(frame.Element("x").Value);
                int y = int.Parse(frame.Element("y").Value);
                int width = int.Parse(frame.Element("width").Value);
                int height = int.Parse(frame.Element("height").Value);

                rectangles.Add(new Rectangle(x, y, width, height));
                Console.WriteLine("Frame: {0}, {1}, {2}, {3}", x, y, width, height);
            }

            SpriteAnimation spriteAnimation = new SpriteAnimation();
            spriteAnimation.initialize(contentManager, textureName, rectangles.ToArray(), fps, rectangles.Count);

            return spriteAnimation;
        }

        public SoundEffect getSoundEffect(string name)
        {
            return contentManager.Load<SoundEffect>(name);
        }
    }
}
