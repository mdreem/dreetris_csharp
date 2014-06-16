using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    class KeyframeStraight : Keyframe
    {
        float velocityScalar = 0;

        public KeyframeStraight(Vector2 start, Vector2 end, float duration)
        {
            _start = start;
            _end = end;
            _current = start;
            Vector2 diff = end - start;
            this.velocityScalar = diff.Length() / duration;
            this.duration = duration;
            _runningTime = duration;

            System.Console.WriteLine("Velocity: {0}; Position: ({1}|{2}).", velocityScalar, _current.X, _current.Y);
        }

        public override double Delay(double time)
        {
            _runningTime += time;
            //TODO: Update position accordingly!
            return _runningTime;
        }

        public override void Reset()
        {
            _current = start;
        }

        public override void Update(GameTime gameTime)
        {
            double time_passed = gameTime.ElapsedGameTime.TotalMilliseconds; ;
            _runningTime -= time_passed;

            if (runningTime < 0)
            {
                _current = end;
                return;
            }

            Vector2 diff = end - start;
            diff.Normalize();
            float distance = velocityScalar * (float)time_passed;

            _current = _current + diff * distance;
        }

        public override List<Vector2> GetPath()
        {
            List<Vector2> res = new List<Vector2>();

            res.Add(start);
            res.Add(end);

            return res;
        }
    }
}
