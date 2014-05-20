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

    public class FileMessageLogger : ILogger
    {
        #region Data
        private string _FileName;
        private StreamWriter _LogFile;

        public string FileName
        {
            get { return _FileName; }
        }
        #endregion

        #region Constructor
        public FileMessageLogger(string fileName)
        {
            _FileName = fileName;
            _LogFile = new StreamWriter(_FileName);
            _LogFile.WriteLine("Session started" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
        }
        #endregion

        #region Public methods
        public void Terminate()
        {
            _LogFile.Close();
        }
        #endregion

        #region ILogger Members

        public void ProcessMessage(string logMessage)
        {
            // save messages to log
            _LogFile.Write("[" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + "] ");
            _LogFile.WriteLine(logMessage);
        }

        public void ProcessError(string logMessage, logLevel severity)
        {
            // here we want to ignore errors and log only messages
            // _LogFile.WriteLine(logMessage);
        }
        #endregion

        

        #region IDisposable patterns
        /* IDisposable patterns, this pattern Dispose implemented and virtual Dispose(bool disposing) is used
         also in System.IO.Stream, it's the best way to implement this according to .NET coding conventions */
        public virtual void Dispose()
        {
            this.Dispose(true); // dispose managed and unmanaged rosources alike!
            GC.SuppressFinalize(this); // avoid to execute finalization of this class from happening two times
        }

        protected virtual void Dispose(bool disposing)
        {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                // clean up managed resources here
                _LogFile.Close();
            }
            // clean up unmanaged resources here
        }

        ~FileMessageLogger()
        {
            Dispose(false);
        }

        #endregion

    }



}
