namespace LogTest
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class AsyncLog : ILog
    {
        private readonly BlockingCollection<LogLine> _lines;
        private bool _exit;
        private bool _quitWithFlush;
        private readonly IFileWriter _writer;
        private int _maxCollectionSize = 1000; //should be taken from configuration

        public AsyncLog(IFileWriter writer)
        {
            _lines = new BlockingCollection<LogLine>();
            
            var runThread = new Thread(MainLoop);
            runThread.Start();
            _writer = writer;
        }

        public void StopWithoutFlush()
        {
            _lines.CompleteAdding();
            _exit = true;
        }

        public void StopWithFlush()
        {
            _lines.CompleteAdding();
            _quitWithFlush = true;
        }

        public void Write(string text)
        {
            if (!_lines.IsAddingCompleted && !ReachedMaxCollectionSize())
                _lines.Add(new LogLine {Text = text, Timestamp = DateTime.Now});
        }
        
        private void MainLoop()
        {
            while (!_exit)
            {
                LogLine logLine = null;
                try
                {
                    logLine = _lines.Take();
                }
                catch (Exception)
                {
                    _exit = true;
                }

                if (logLine != null && (!_exit || _quitWithFlush))
                {
                    _writer.Write(logLine);
                }

                if (_quitWithFlush && _lines.Count == 0)
                    _exit = true;
            }

            _lines?.Dispose();
            _writer?.Dispose();
        }

        private bool ReachedMaxCollectionSize()
        {
            return _lines.Count > _maxCollectionSize;
        }
    }
}