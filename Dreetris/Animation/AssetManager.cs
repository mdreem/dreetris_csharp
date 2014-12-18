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

        public AssetManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public Sprite getSprite(string name)
        {
            Console.WriteLine(string.Format("Getting sprite data for: {0}", name));
            /*
            var width = int.Parse((from sprite in XDocument.Load("Sprites.xml").Descendants("Sprite")
                                   where sprite.Element("name").Value == name
                                   select sprite.Element("width").Value).ToList().First());

            var height = int.Parse((from sprite in XDocument.Load("Sprites.xml").Descendants("Sprite")
                                    where sprite.Element("name").Value == name
                                    select sprite.Element("height").Value).ToList().First());
            */
            var textureName = (from sprite_data in XDocument.Load("Sprites.xml").Descendants("Sprite")
                               where sprite_data.Element("name").Value == name
                               select sprite_data.Element("texture").Value).ToList().First();



            Console.WriteLine(string.Format("Texture name: {0}", textureName));

            //Texture2D texture = contentManager.Load<Texture2D>(textureName);
            Sprite sprite = new Sprite();
            sprite.initialize(contentManager, textureName);

            return sprite;
        }

        public SoundEffect getSoundEffect(string name)
        {
            return contentManager.Load<SoundEffect>(name);
        }
    }
}
