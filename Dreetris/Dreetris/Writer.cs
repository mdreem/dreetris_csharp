using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Dreetris
{
    class Writer
    {
        Stopwatch stopwatch = new Stopwatch();
        long interval;
        long last_tick;

        public Writer(long interval)
        {
            stopwatch.Start();
            this.interval = interval;
            last_tick = stopwatch.ElapsedMilliseconds;
        }

        bool elapsed()
        {
            long now = stopwatch.ElapsedMilliseconds;
            return (now - last_tick) < interval;
        }

        void next_tick()
        {
            last_tick = stopwatch.ElapsedMilliseconds;
        }

        public void Write(string format, params object[] values)
        {
            if (!elapsed())
            {
                System.Console.WriteLine(format, values);
                next_tick();
            }
        }
    }
}
