using System;

namespace Nebula.Infrastructure
{
    public static class SystemClock
    {
        private static DateTime? mockedDate;

        public static DateTime Now
        {
            get { return mockedDate.HasValue ? mockedDate.Value : DateTime.Now; }
        }

        public static void Mock(DateTime date)
        {
            mockedDate = date;
        }

        public static void Reset()
        {
            mockedDate = null;
        }
    }
}