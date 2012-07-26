namespace Nebula.Infrastructure
{
    public static class SystemContext
    {
        private static volatile ISystemClock currentClock;

        public static ISystemClock Clock
        {
            get { return currentClock ?? (currentClock = new RealTimeSystemClock()); }
            set { currentClock = value; }
        }
    }
}