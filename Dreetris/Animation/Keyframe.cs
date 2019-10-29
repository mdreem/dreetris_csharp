using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    public abstract class Keyframe
    {
        protected Vector2 _start;
        protected Vector2 _end;
        protected Vector2 _current;

        protected double duration;
        protected double _runningTime;

        public Vector2 Start
        {
            get { return _start; }
        }

        public Vector2 End
        {
            get { return _end; }
        }

        public Vector2 Current
        {
            get { return _current; }
        }

        public double RunningTime
        {
            get { return _runningTime; }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Reset();
        public abstract double Delay(double time);
        public abstract List<Vector2> GetPath();
    }
}
