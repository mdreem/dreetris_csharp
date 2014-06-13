using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    class Keyframe_Bezier : Keyframe
    {
        float velocity_scalar = 0;
        List<BezierCurve> curves = new List<BezierCurve>();

        public Keyframe_Bezier(BezierCurve curve, float duration)
        {
            curves.Add(curve);

            _start = curve.start;
            _end = curve.end;
            _current = start;
            this.velocity_scalar = curve.length / duration;
            this.duration = duration;
            _running_time = duration;

            System.Console.WriteLine("Velocity: {0}; Position: ({1}|{2}).", velocity_scalar, _current.X, _current.Y);
        }

        public override double delay(double time)
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

            float t_param = ((float)(duration - _running_time)) * velocity_scalar / curves[0].length;

            //TODO: multiple curves!
            _current = curves[0].get_position(t_param);
        }

        public List<Vector2> get_hull()
        {
            List<Vector2> res = new List<Vector2>();

            foreach(var curve in curves)
            {
                res.AddRange(curve.get_hull());
            }

            return res;
        }

        public override List<Vector2> get_path()
        {
            List<Vector2> res = new List<Vector2>();

            foreach (var curve in curves)
            {
                res.AddRange(curve.subdivide());
            }

            return res;
        }
    }
}
