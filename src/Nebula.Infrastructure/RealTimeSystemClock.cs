using System;

namespace Nebula.Infrastructure
{
    public class RealTimeSystemClock : ISystemClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}