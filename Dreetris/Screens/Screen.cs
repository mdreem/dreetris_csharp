using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dreetris.Screens
{
    public abstract class Screen : DrawableGameComponent
    {
        protected bool _isActive = false;
        public bool isActive { get { return _isActive; } }

        public enum ScreenState
        {
        }

        public ScreenManager screenManager { get; set;}
        public Boolean visible { get; set; }

        public Screen(Game game, ScreenManager screenManager)
            : base(game)
        {
            this.screenManager = screenManager;
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
        /*
        public virtual void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        public virtual void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }*/
    }
}
