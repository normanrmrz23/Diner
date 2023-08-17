using System;

namespace Diner.Persistence
{
    internal static class SystemTimeService
    {
        internal static Func<DateTimeOffset> NowFunc { get; set; } = () => DateTimeOffset.Now;

        public static DateTimeOffset Now()
        {
            return NowFunc();
        }
    }
}

