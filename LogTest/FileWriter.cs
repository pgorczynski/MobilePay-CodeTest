namespace LogTest
{
    using System;
    using System.IO;

    public class FileWriter : IFileWriter
    {
        private StreamWriter _writer;
        private DateTime? lastDateTime;
        private readonly string _logFileDirectory;

        public FileWriter()
        {
            _logFileDirectory = @"C:\LogTest";
        }

        public FileWriter(string logFileDirectory)
        {
            _logFileDirectory = logFileDirectory;
        }

        public void Write(LogLine logLine)
        {
            try
            {
                CreateFileIfNeeded();

                _writer.Write($"{logLine.Timestamp:yyyy-MM-dd HH:mm:ss:fff}\t{logLine.LineText()}\t{Environment.NewLine}");
            }
            catch (Exception)
            {
                //later can be added some exception handling logic (writing to EventLog for an example)
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer?.Dispose();
            }
        }

        private void CreateFileIfNeeded()
        {
            if (ApplicationTime.Today == lastDateTime)
                return;

            CreateDirectoryIfNeeded();
            lastDateTime = ApplicationTime.Today;
            var fileName = _logFileDirectory + @"\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log";
            _writer = File.AppendText(fileName);

            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            _writer.AutoFlush = true;
        }

        private void CreateDirectoryIfNeeded()
        {
            if (!Directory.Exists(_logFileDirectory))
                Directory.CreateDirectory(_logFileDirectory);
        }
    }
}