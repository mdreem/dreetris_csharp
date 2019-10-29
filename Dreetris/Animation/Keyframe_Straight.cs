using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    class KeyframeStraight : Keyframe
    {
        float velocityScalar;

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
            _current = Start;
        }

        public override void Update(GameTime gameTime)
        {
            double time_passed = gameTime.ElapsedGameTime.TotalMilliseconds; ;
            _runningTime -= time_passed;

            if (RunningTime < 0)
            {
                _current = End;
                return;
            }

            Vector2 diff = End - Start;
            diff.Normalize();
            float distance = velocityScalar * (float)time_passed;

            _current = _current + diff * distance;
        }

        public override List<Vector2> GetPath()
        {
            List<Vector2> res = new List<Vector2>();

            res.Add(Start);
            res.Add(End);

            return res;
        }
    }
}
