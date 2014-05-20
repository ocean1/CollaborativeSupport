using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Logger;

namespace CommonUtils.Configuration
{
    /// <summary>
    /// this is the abstract class that implements the function usefull when working with options
    /// </summary>
    /// <typeparam name="T">the type that inherits from options and contains the definition of the
    /// XML elements of the config file</typeparam>
    public abstract class Options<T> where T : Options<T>, new()
    {
        public static string defaultConfigFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\options.xml";

        private static T _Options = null;
        private static readonly object _lock = new object();
        private bool _disposed;

        public bool rimemberForNextSession = false;

        public static Options<T> Instance
        {
            get
            {
                // If this is the first time we’re referring to the
                // singleton object, the private variable will be null.
                // This is done to avoid locking every time we need to
                // get the Instance
                if (_Options == null)
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
                        if (_Options == null)
                        {
                            try
                            {
                                if (File.Exists(defaultConfigFile))
                                {
                                    LoadFromFile(out _Options, defaultConfigFile);
                                    _Options.rimemberForNextSession = true;
                                }
                                else
                                    _Options = new T();

                            }
                            catch (OptionException)
                            {
                                _Options = new T();
                                //throw;
                            }
                        }
                    }
                }
                return _Options;
            }
        }

        protected Options()
        {
        }

        /// <summary>
        /// Load XML config from file
        /// </summary>
        /// <param name="options">the output variable</param>
        /// <param name="configFile">the config file location</param>
        /// <returns>returns true if the file has been loaded succesfully else false</returns>
        private static void LoadFromFile(out T options, string configFile)
        {
            TextReader textReader = null;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                textReader = new StreamReader(configFile);
                options = (T)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch (Exception ex)
            {
                if (textReader != null) textReader.Close();
                throw new OptionException("Cannot load configuration from config file", ex);
            }

        }

        private static void SaveToFile(T options, string configFile)
        {
            TextWriter textWriter = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                textWriter = new StreamWriter(configFile);
                serializer.Serialize(textWriter, options);
                textWriter.Close();
            }
            catch (Exception ex)
            {
                if (textWriter != null) textWriter.Close();
                throw new OptionException("Cannot save configuration to config file", ex);
            }
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
        protected void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed == false)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                    }
                    // clean up unmanaged resources here
                    if (rimemberForNextSession)
                        SaveToFile(_Options, defaultConfigFile);
                    else
                        if (File.Exists(defaultConfigFile))
                        {
                            try
                            {
                                File.Delete(defaultConfigFile);
                            }
                            catch (Exception ex)
                            {
                                MyLogger.Instance.AddError(ex, logLevel.ERROR);
                                //MessageBox.Show(this, "Error deleting configuration file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                }
                _disposed = true;
            }
        }

        ~Options()
        {
            Dispose(false);
        }
        #endregion


    }

    /// <summary>
    /// the custom exception we're going to throw if there are errors
    /// implements [Serializable] because Exception implements ISerializable interface
    /// (not implementing it was shown as an error by code analysis)
    /// </summary>
    [Serializable]
    public class OptionException : Exception
    {
        public OptionException()
        {
        }

        public OptionException(string message)
            : base(message)
        {
        }

        public OptionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public OptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }


}
