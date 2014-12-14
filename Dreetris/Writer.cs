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
        long lastTick;

        public Writer(long interval)
        {
            stopwatch.Start();
            this.interval = interval;
            lastTick = stopwatch.ElapsedMilliseconds;
        }

        bool Elapsed()
        {
            long now = stopwatch.ElapsedMilliseconds;
            return (now - lastTick) < interval;
        }

        void NextTick()
        {
            lastTick = stopwatch.ElapsedMilliseconds;
        }

        public void Write(string format, params object[] values)
        {
            if (!Elapsed())
            {
                System.Console.WriteLine(format, values);
                NextTick();
            }
        }
    }
}
