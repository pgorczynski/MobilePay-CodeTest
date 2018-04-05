using System.Threading;
using LogTest;
using Moq;

namespace LogTests
{
    public class LogTestsFixture
    {
        private int _writesCount = 5;
        private readonly Mock<IFileWriter> _writerMock;
        public AsyncLog Log { get; }

        public LogTestsFixture()
        {
            _writerMock = new Mock<IFileWriter>();
            _writerMock.Setup(a => a.Write(It.IsAny<LogLine>())).Callback(() => { Thread.Sleep(2); });
            Log = new AsyncLog(_writerMock.Object);
        }

        public void write_multiple_items_to_log()
        {
            for (int i = 0; i < _writesCount; ++i)
            {
                Log.Write("test" + i);
            }
        }

        public void verify_if_outstanding_logs_were_written()
        {
            Thread.Sleep(20);
            _writerMock.Verify(a => a.Write(It.IsAny<LogLine>()), Times.Exactly(_writesCount));
        }

        public void verify_if_outstanding_logs_were_not_written()
        {
            _writerMock.Verify(a => a.Write(It.IsAny<LogLine>()), Times.AtMost(_writesCount - 1));
        }
    }
}