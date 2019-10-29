using System;
using Microsoft.Xna.Framework;

namespace Dreetris.Screens
{
    public abstract class Screen : DrawableGameComponent
    {
        protected bool _isActive = false;
        protected GameObjects gameObjects;

        public bool isActive { get { return _isActive; } }
        public ScreenManager screenManager { get { return gameObjects.ScreenManager; } }
        public Boolean visible { get; set; }

        public Screen(GameObjects gameObjects)
            : base(gameObjects.game)
        {
            this.gameObjects = gameObjects;
            visible = true;
        }

        public virtual void Activate()
        {
            _isActive = true;
        }

        public virtual void Deactivate()
        {
            _isActive = false;
        }
    }
}
