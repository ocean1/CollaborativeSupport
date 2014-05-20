using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Logger
{
    /// <summary>
    /// Defines the level of logging
    /// logging most errors includes severe errors
    /// logging warnings includes logging errors
    /// </summary>
    [Serializable]
    public enum logLevel
    {
        [System.Xml.Serialization.XmlEnum("0")]
        SEVERE_ERROR,
        [System.Xml.Serialization.XmlEnum("1")]
        ERROR,
        [System.Xml.Serialization.XmlEnum("2")]
        WARNING
    }

    public class MyLogger
    {

        #region Data
        private static readonly object _lock = new object();
        private static readonly object _lockWriter = new object();

        private bool _disposed;

        private static MyLogger _Logger = null;

        /// <summary>
        /// The Enabled variable defines the behaviour of the logger
        /// permitting to start/stop logging on the fly.
        /// Since more than one thread is probably going to access this variable
        /// atomic operations have to be used to access it hence why we defined it
        /// as volatile.
        /// That's acceptable since the intended use is to permit the user to
        /// stop/start the logger whenever he wants to from an option dialog
        /// and we don't really care about loosing/logging one more message.
        /// </summary>
        public static volatile bool Enabled = false;

        public static MyLogger Instance
        {
            get
            {
                // If this is the first time we’re referring to the
                // singleton object, the private variable will be null.
                // This is done to avoid locking every time we need to
                // get the Instance
                if (_Logger == null)
                {
                    // for thread safety, lock an object when
                    // instantiating the new Logger object. This prevents
                    // other threads from performing the same block at the
                    // same time.
                    lock (_lock)
                    {
                        // Two or more threads might have found a null
                        // mLogger and are therefore trying to create a 
                        // new one. One thread will get to lock first, and
                        // the other one will wait until mLock is released.
                        // Once the second thread can get through, mLogger
                        // will have already been instantiated by the first
                        // thread so test the variable again. 
                        if (_Logger == null)
                        {
                            _Logger = new MyLogger();
                        }
                    }
                }
                return _Logger;
            }
        }

        private List<ILogger> _Observers;

        #endregion

        #region Constructor
        private MyLogger()
        {
            _Observers = new List<ILogger>();
        }
        #endregion

        #region Public methods
        public void RegisterObserver(ILogger observer)
        {
            if (!_Observers.Contains(observer))
            {
                _Observers.Add(observer);
            }
        }

        public void AddMessage(string message)
        {
            // Apply some basic formatting like the current timestamp
            string formattedMessage = string.Format("[{0}] - {1}", DateTime.Now.ToString(), message);
            foreach (ILogger observer in _Observers)
            {
                observer.ProcessMessage(formattedMessage);
            }
        }

        public void AddError(Exception ex, logLevel severity)
        {
            string formattedMessage = string.Format("[{0}] - {1}", DateTime.Now.ToString(), ex.Message);
            StringBuilder message = new StringBuilder(formattedMessage);
            
            string trace = ex.StackTrace;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message.Append("\r\n" + ex.Message);
            }
            message.Append("\r\nStack trace: " + trace);

            AddError(message.ToString(), severity);
        }

        public void AddError(string message, logLevel severity)
        {
            if (Enabled == true)
                lock (_lockWriter)
                {
                    foreach (ILogger observer in _Observers)
                    {
                        observer.ProcessError(message, severity);
                    }
                }
        }

        #endregion
    
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
        protected void Dispose(bool disposing)
        {
            lock (_lock)
            lock (_lockWriter)
            {
                if (_disposed == false)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // clean up managed resources here
                    }

                    // clean up unmanaged resources here
                    foreach( ILogger observer in _Observers ){
                        observer.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~MyLogger()
        {
            Dispose(false);
        }
        #endregion
    }
}
