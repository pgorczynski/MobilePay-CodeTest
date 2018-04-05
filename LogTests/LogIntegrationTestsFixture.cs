using System;
using System.IO;
using System.Threading;
using LogTest;
using Xunit;

namespace LogTests
{
    public class LogIntegrationTestsFixture : IDisposable
    {
        public AsyncLog Log { get; }
        private readonly string _directory;
        private readonly FileWriter _fileWriter;

        public LogIntegrationTestsFixture()
        {
            _directory = Path.Combine(Directory.GetCurrentDirectory(), "log" + Guid.NewGuid());
            _fileWriter = new FileWriter(_directory);
            Log = new AsyncLog(_fileWriter);
        }

        public void check_if_log_file_was_created()
        {
            var files = GetLogFiles();

            Assert.Single(files);
        }

        public void check_if_log_file_contains_N_lines(int linesCount)
        {
            var files = GetLogFiles();

            Assert.Single(files);

            using (StreamReader reader = new StreamReader(files[0]))
            {
                var lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                Assert.Equal(linesCount, lines.Length);
            }
        }
        
        public void check_if_two_log_file_were_created()
        {
            var files = GetLogFiles();

            Assert.Equal(2, files.Length);
        }

        private string[] GetLogFiles()
        {
            Thread.Sleep(20);
            _fileWriter.Dispose();

            return Directory.GetFiles(_directory);
        }

        public void Dispose()
        {
            if (Directory.Exists(_directory))
                Directory.Delete(_directory, true);
        }
    }
}