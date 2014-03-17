namespace Nebula.Infrastructure
{
    public static class SystemContext
    {
        private static volatile ISystemClock _currentClock;

        public static ISystemClock Clock
        {
            get { return _currentClock ?? (_currentClock = new RealTimeSystemClock()); }
            set { _currentClock = value; }
        }
    }
}