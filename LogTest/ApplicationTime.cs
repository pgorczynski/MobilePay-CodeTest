using System;

namespace LogTest
{
    public static class ApplicationTime
    {
        private static readonly Func<DateTime> _defaultLogic = () => DateTime.Now;

        private static Func<DateTime> _current = _defaultLogic;

        /// <summary>
        /// Returns current date/time, correct in application context
        /// </summary>
        /// <remarks>Normally this would return <see cref="DateTime.Now" />
        /// , but can be changed to make testing easier.</remarks>
        public static DateTime Now
        {
            get { return _current(); }
        }
        /// <summary>
        /// Returns current date, correct in application context
        /// </summary>
        /// <remarks>Normally this would return <see cref="DateTime.Today" />
        /// , but can be changed to make testing easier.</remarks>
        public static DateTime Today
        {
            get { return _current().Date; }
        }

        /// <summary>
        /// Changes logic behind <see cref="Now" />.
        /// Useful in scenarios where time needs to be defined upfront, like unit tests.
        /// </summary>
        /// <remarks>Be sure you know what you are doing when calling this method.</remarks>
        public static void ReplaceCurrentTimeLogic(Func<DateTime> newCurrentTimeLogic)
        {
            _current = newCurrentTimeLogic;
        }

        /// <summary>
        /// Reverts current logic to the default logic.
        /// Useful in scenarios where unit test changed logic and should rever system to previous state.
        /// </summary>
        /// <remarks>Be sure you know what you are doing when calling this method.</remarks>
        public static void RevertToDefaultLogic()
        {
            _current = _defaultLogic;
        }
    }
}