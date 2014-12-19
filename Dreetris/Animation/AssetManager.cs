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
                sprite = getSpriteFromData(data);
                Console.WriteLine("Type: nix");

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
            return null;
        }

        public SoundEffect getSoundEffect(string name)
        {
            return contentManager.Load<SoundEffect>(name);
        }
    }
}
