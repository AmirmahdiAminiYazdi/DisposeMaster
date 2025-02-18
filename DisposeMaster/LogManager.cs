using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeMaster
{
    // Class responsible for logging messages to a file while properly handling resource disposal
    public class LogManager : IDisposable
    {
        private StreamWriter _logWriter; // stream for writing log messages to a file
        private bool _disposed = false; // Flag to track disposal status

        public LogManager(string filepath)
        {
            _logWriter = new StreamWriter(filepath, append: true);
        }
        // Writes a log message to the file
        public void WriteLog(string message)
        {
            if (_disposed)
                throw new ObjectDisposedException("LogManger");
            _logWriter.WriteLine($"{DateTime.Now}:{message}");
            _logWriter.Flush();

        }

        // Public method to release resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Prevent finalizer from running since we handled disposal
        }

        // Handles the resource cleanup logic
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _logWriter?.Close();// Close the stream if disposing
                    _logWriter?.Dispose();// Dispose the stream properly
                }
                _disposed = true;
            }
        }
        // Finalizer to release unmanaged resources if Dispose was not called
        ~LogManager()
        { Dispose(false); }

    }
}
