using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using CommonUtils.Configuration;
using Logger;

namespace Client
{
    [XmlRoot("Server Options")]
    public sealed class ClientOptions : Options<ClientOptions>
    {
        //public static string defaultConfigFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\serverConfig.xml";

        public new static ClientOptions Instance
        {
            get
            {
                return (ClientOptions)Options<ClientOptions>.Instance;
            }
        }

        // defaults for the options
        private string _username = "username";
        private string _password = "password";
        private string _serverURI = "127.0.0.1:6500";
        private bool _logErrorEnable = true;
        private bool _logMessageEnable = false;
        private Logger.logLevel _loggingLevel = logLevel.WARNING;

        private string _errorLogFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\errorLog.txt";
        private string _messageLogFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\messageLog.txt";

        
        [XmlElement("username")]
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        [XmlElement("password")]
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        [XmlElement("port")]
        public string serverURI
        {
            get { return _serverURI; }
            set { _serverURI = value; }
        }

        [XmlElement("logErrorEnable")]
        public bool logErrorEnable
        {
            get { return _logErrorEnable; }
            set { _logErrorEnable = value; }
        }

        [XmlElement("logMessageEnable")]
        public bool logMessageEnable
        {
            get { return _logMessageEnable; }
            set { _logMessageEnable = value; }
        }

        [XmlElement("loggingLevel")]
        public Logger.logLevel loggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        [XmlElement("errorLogFile")]
        public string errorLogFile
        {
            get { return _errorLogFile; }
            set { _errorLogFile = value; }
        }

        [XmlElement("messageLogFile")]
        public string messageLogFile
        {
            get { return _messageLogFile; }
            set { _messageLogFile = value; }
        }

    }

}
