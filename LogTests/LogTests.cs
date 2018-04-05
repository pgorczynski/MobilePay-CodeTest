using Xunit;

namespace LogTests
{
    public class LogTests
    {
        [Fact]
        public void calling_StopWithFlush_should_write_outstanding_logs()
        {
            _fixture.write_multiple_items_to_log();

            _fixture.Log.StopWithFlush();

            _fixture.verify_if_outstanding_logs_were_written();
        }

        [Fact]
        public void calling_StopWithoutFlush_should_not_write_outstanding_logs()
        {
            _fixture.write_multiple_items_to_log();

            _fixture.Log.StopWithoutFlush();

            _fixture.verify_if_outstanding_logs_were_not_written();
        }
        
        public LogTests()
        {
            _fixture = new LogTestsFixture();
        }

        private readonly LogTestsFixture _fixture;
    }
}