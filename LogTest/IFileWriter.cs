using System;

namespace LogTest
{
    public interface IFileWriter : IDisposable
    {
        void Write(LogLine logLine);
    }
}