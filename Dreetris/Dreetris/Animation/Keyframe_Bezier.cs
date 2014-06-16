using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris.Animation
{
    class Keyframe_Bezier : Keyframe
    {
        float velocityScalar = 0;
        List<BezierCurve> curves = new List<BezierCurve>();

        public Keyframe_Bezier(BezierCurve curve, float duration)
        {
            curves.Add(curve);

            _start = curve.start;
            _end = curve.end;
            _current = start;
            this.velocityScalar = curve.length / duration;
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
            double timePassed = gameTime.ElapsedGameTime.TotalMilliseconds; ;
            _runningTime -= timePassed;

            if (runningTime < 0)
            {
                _current = end;
                return;
            }

            float t_param = ((float)(duration - _runningTime)) * velocityScalar / curves[0].length;

            //TODO: multiple curves!
            _current = curves[0].GetPosition(t_param);
        }

        public List<Vector2> GetHull()
        {
            List<Vector2> res = new List<Vector2>();

            foreach(var curve in curves)
            {
                res.AddRange(curve.GetHull());
            }

            return res;
        }

        public override List<Vector2> GetPath()
        {
            List<Vector2> res = new List<Vector2>();

            foreach (var curve in curves)
            {
                res.AddRange(curve.Subdivide());
            }

            return res;
        }
    }
}
