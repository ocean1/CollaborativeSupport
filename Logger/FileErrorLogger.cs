using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    public class FileErrorLogger : ILogger, IDisposable
    {
        #region Data
        private string _FileName;
        private StreamWriter _LogFile;
        private readonly object _lock = new object();
        private bool _disposed { get; set; }

        public bool Error;
       
        public string FileName
        {
            get { return _FileName; }
        }
        #endregion

        #region Constructor
        public FileErrorLogger(string fileName)
        {

            try
            {
                _FileName = fileName;
                _LogFile = new StreamWriter(_FileName);
                _LogFile.AutoFlush = true;
                _LogFile.WriteLine("Session started on " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + "\n");
            }
            catch (Exception ex)
            {
                MyLogger.Instance.AddError(ex, logLevel.ERROR);
            }
            Error = true;
        }
        #endregion

        #region ILogger Members

        public void ProcessMessage(string logMessage)
        {
            //in this case we want to ignore logging messages
        }

        public void ProcessError(string logMessage, logLevel severity)
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    _LogFile.WriteLine("-------------------------------------------------------------------------------------------------");
                    _LogFile.WriteLine(logMessage);
                    _LogFile.WriteLine("");
                }
            }
        }
        #endregion

        public void Terminate()
        {
            this.Dispose();
        }

        
        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true); // dispose managed and unmanaged rosources alike!
            GC.SuppressFinalize(this); // avoid to execute finalization of this class from happening two times
            //base.Dispose();
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">tells if the method has been called directly (true)
        /// or indirectly by the runtime (false)</param>
        private void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed == false)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // clean up managed resources here
                        if(!Error) _LogFile.Close();
                    }

                    // clean up unmanaged resources here   

                }
                _disposed = true;
            }
        }

        ~FileErrorLogger()
        {
            Dispose(false);
        }

        #endregion

    }

}
