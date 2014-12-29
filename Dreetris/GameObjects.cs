using Dreetris.Animation;
using Dreetris.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris
{
    public class GameObjects
    {
        public Game game { get; set; }
        public ScreenManager screenManager { get; set; }
        public AssetManager assetManager { get; set; }

        public GameObjects(Game game, ScreenManager screenManager, AssetManager assetManager)
        {
            this.game = game;
            this.screenManager = screenManager;
            this.assetManager = assetManager;
        }
    }
}
