using System;
using System.Threading;
using Xunit;

namespace LogTests
{
    public class LogIntegrationTests
    {
        [Fact]
        public void calling_Write_should_write_something()
        {
            act();

            _fixture.check_if_log_file_contains_N_lines(3);
        }

        [Fact]
        public void calling_Write_after_midnight_should_create_new_file()
        {
            using (new TimeOverride(_timeDuringDay))
            {
                act();
                Thread.Sleep(20);
            }

            using (new TimeOverride(_justAfterMidnight))
            {
                act();

                _fixture.check_if_two_log_file_were_created();
            }
        }

        [Fact]
        public void calling_Write_before_midnight_should_not_create_new_file()
        {
            using (new TimeOverride(_timeDuringDay))
            {
                act();
            }

            using (new TimeOverride(_justBeforeMidnight))
            {
                act();
            }

            _fixture.check_if_log_file_was_created();
        }

        private void act()
        {
            _fixture.Log.Write("test");
        }

        public LogIntegrationTests()
        {
            _fixture = new LogIntegrationTestsFixture();
        }

        private readonly LogIntegrationTestsFixture _fixture;
        private readonly DateTime _justBeforeMidnight = new DateTime(2017, 12, 13, 23, 59, 59);
        private readonly DateTime _justAfterMidnight = new DateTime(2017, 12, 14, 0, 0, 0);
        private readonly DateTime _timeDuringDay = new DateTime(2017, 12, 13, 10, 0, 0);
    }
}
