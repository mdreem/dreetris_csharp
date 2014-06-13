using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    public abstract class Keyframe
    {
        protected Vector2 _start;
        protected Vector2 _end;
        protected Vector2 _current;

        protected double duration = 0;
        protected double _running_time = 0;

        public Vector2 start
        {
            get { return _start; }
        }

        public Vector2 end
        {
            get { return _end; }
        }

        public Vector2 current
        {
            get { return _current; }
        }

        public double running_time
        {
            get { return _running_time; }
        }

        public abstract void Update(GameTime gameTime);
        public abstract void reset();
        public abstract double delay(double time);
        public abstract List<Vector2> get_path();
    }
}
