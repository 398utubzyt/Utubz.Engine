using System;

namespace Utubz.Async
{
    public sealed class TimeContext : Object
    {
        private double prev;
        private double now;
        private DateTime start;
        private DateTime origin;

        public double Time64 => now;
        public double Delta64 => now - prev;

        public double Since(double time)
        {
            return now - time;
        }

        public void Modify(double time)
        {
            origin = start.AddSeconds(time);  
        }

        public float Time => (float)now;
        public float Delta => (float)Delta64;

        public float Since(float time)
        {
            return Time - time;
        }

        public void Modify(float time)
        {
            origin = start.AddSeconds(time);
        }

        public void Update()
        {
            prev = now;
            now = DateTime.Now.Subtract(origin).TotalSeconds;
        }

        internal TimeContext()
        {
            start = DateTime.Now;
            origin = start;
            now = DateTime.Now.Subtract(origin).TotalSeconds;
        }
    }
}
