using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    class Keyframe_Straight : Keyframe
    {
        float velocity_scalar = 0;

        public Keyframe_Straight(Vector2 start, Vector2 end, float duration)
        {
            _start = start;
            _end = end;
            _current = start;
            Vector2 diff = end - start;
            this.velocity_scalar = diff.Length() / duration;
            this.duration = duration;
            _running_time = duration;

            System.Console.WriteLine("Velocity: {0}; Position: ({1}|{2}).", velocity_scalar, _current.X, _current.Y);
        }

        public double delay(double time)
        {
            _running_time += time;
            //TODO: Update position accordingly!
            return _running_time;
        }
        
        public override void reset()
        {
            _current = start;
        }

        public override void Update(GameTime gameTime)
        {
            double time_passed = gameTime.ElapsedGameTime.TotalMilliseconds; ;
            _running_time -= time_passed;

            if (running_time < 0)
            {
                _current = end;
                return;
            }

            Vector2 diff = end - start;
            diff.Normalize();
            float distance = velocity_scalar * (float)time_passed;

            _current = _current + diff * distance;
        }
    }
}
