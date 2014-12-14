using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    public class BezierCurve
    {
        Vector2 pStart, pEnd;
        Vector2 control1, control2;
        float _length;

        public float length
        {
            get { return _length;  }
        }

        public Vector2 start
        {
            get { return pStart; }
        }

        public Vector2 end
        {
            get { return pEnd; }
        }

        public BezierCurve(Vector2 pStart, Vector2 pEnd,
                           Vector2 control1, Vector2 control2)
        {
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.control1 = control1;
            this.control2 = control2;

            _length = ComputeLength();
        }

        /// <summary>
        /// Returns the position of a point on the curve with parameter t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 GetPosition(float t)
        {
            float sing = (1 - t);
            float sq = sing * sing;
            float cub = sq * sing;
            Vector2 cur = cub * pStart
                        + 3 * sq * t * control1
                        + 3 * sing * t * t * control2
                        + t * t * t * pEnd;

            return cur;
        }

        /// <summary>
        /// Computes the length of the curve by subdividing it into straight lines.
        /// </summary>
        /// <returns></returns>
        float ComputeLength()
        {
            List<Vector2> parts = Subdivide();
            float length = 0;
            for (int i = 0; i < parts.Count - 1; i++)
            {
                length += (parts[i + 1] - parts[i]).Length();
            }

            return length;
        }

        /// <summary>
        /// Subdivides the Bezier curve into straight lines.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<Vector2> Subdivide(int n = 100)
        {
            List<Vector2> ret = new List<Vector2>();
            float steps = 1 / (float)n;

            //System.Console.WriteLine("***");
            for (int i = 0; i <= n; i++)
            {
                //System.Console.WriteLine("({0},{1}) - {2}", get_position(steps * i).X, get_position(steps * i).Y, steps * i);
                ret.Add(GetPosition(steps * i));
            }
            return ret;
        }

        /// <summary>
        /// Returns the hull of the bezier curve as defined by its start- and endpoint and the two control points.
        /// </summary>
        /// <returns></returns>
        public List<Vector2> GetHull()
        {
            List<Vector2> ret = new List<Vector2>();
            ret.Add(pStart);
            ret.Add(control1);
            ret.Add(control2);
            ret.Add(pEnd);
            return ret;
        }
    }
}
