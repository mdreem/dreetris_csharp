using Dreetris.Animation;
using Dreetris.Screens;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    public class GameObjects
    {
        public Game game { get; set; }
        public ScreenManager ScreenManager { get; set; }
        public AssetManager AssetManager { get; set; }

        public GameObjects(Game game, ScreenManager screenManager, AssetManager assetManager)
        {
            this.game = game;
            this.ScreenManager = screenManager;
            this.AssetManager = assetManager;
        }
    }
}
