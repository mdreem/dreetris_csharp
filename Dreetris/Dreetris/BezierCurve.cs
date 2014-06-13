﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dreetris
{
    public class BezierCurve
    {
        Vector2 p_start, p_end;
        Vector2 control_1, control_2;

        public BezierCurve(Vector2 p_start, Vector2 p_end,
                           Vector2 control_1, Vector2 control_2)
        {
            this.p_start = p_start;
            this.p_end = p_end;
            this.control_1 = control_1;
            this.control_2 = control_2;

            //_t = 0;
        }

        public Vector2 get_position(float t)
        {
            float sing = (1 - t);
            float sq = sing * sing;
            float cub = sq * sing;
            Vector2 cur = cub * p_start
                        + 3 * sq * t * control_1
                        + 3 * sing * t * t * control_2
                        + t * t * t * p_end;

            return cur;
        }

        public List<Vector2> subdivide(int n = 100)
        {
            List<Vector2> ret = new List<Vector2>();
            float steps = 1 / (float)n;

            //System.Console.WriteLine("***");
            for (int i = 0; i <= n; i++)
            {
                //System.Console.WriteLine("({0},{1}) - {2}", get_position(steps * i).X, get_position(steps * i).Y, steps * i);
                ret.Add(get_position(steps * i));
            }
            return ret;
        }

        public List<Vector2> get_hull()
        {
            List<Vector2> ret = new List<Vector2>();
            ret.Add(p_start);
            ret.Add(control_1);
            ret.Add(control_2);
            ret.Add(p_end);
            return ret;
        }
    }
}
