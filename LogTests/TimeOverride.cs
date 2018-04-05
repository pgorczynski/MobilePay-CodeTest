using System;
using LogTest;

namespace LogTests
{
    public class TimeOverride : IDisposable
    {
        public TimeOverride(DateTime newDate)
        {
            ApplicationTime.ReplaceCurrentTimeLogic(() => newDate);
        }

        public void Dispose()
        {
            ApplicationTime.RevertToDefaultLogic();
        }
    }
}