using Microsoft.Xna.Framework;
using System;

namespace Dreetris.Screens
{
	/// <summary>
	/// Screen is the base class for the screen management system.
	/// The class ScreenManager is responsible for managing the screens.
	/// <see cref="Dreetris.Screens.ScreenManager"/>
	/// </summary>
    public abstract class Screen : DrawableGameComponent
    {
        protected bool _isActive = false;
	    protected GameObjects gameObjects;
	
		public bool isActive { get { return _isActive; } }
        public ScreenManager screenManager { get { return gameObjects.screenManager;} }
        public Boolean visible { get; set; }

        public Screen(GameObjects gameObjects)
            : base(gameObjects.game)
        {
            this.gameObjects = gameObjects;
            visible = true;
        }

        public virtual void activate()
        {
            _isActive = true;
        }

        public virtual void deactivate()
        {
            _isActive = false;
        }
    }
}
