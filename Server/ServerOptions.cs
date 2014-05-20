using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using System.Windows.Forms;
using CommonUtils;
using CommonUtils.Configuration;
using Logger;

namespace Server
{

    [XmlRoot("Server Options")]
    public sealed class ServerOptions : Options<ServerOptions>
    {

        public new static ServerOptions Instance
        {
            get
            {
                return (ServerOptions)Options<ServerOptions>.Instance;
            }
        }

        // defaults for the options
        private string _username = "Admin";
        private string _password = "password";
        private int _port = 6500;
        private bool _logErrorEnable = true;
        private bool _logMessageEnable = false;
        private Logger.logLevel _loggingLevel = logLevel.WARNING;
        private int _maxConnections = 4;

        private Keys _enableVideoSendKey = Keys.Control | Keys.Alt | Keys.S;

        public Keys EnabledVideoSendKey
        {
            get { return _enableVideoSendKey; }
            set { _enableVideoSendKey = value; }
        }

        private string _errorLogFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\errorLog.txt";
        private string _messageLogFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\messageLog.txt";

        /*[XmlElement("configFile")]
        public string configFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }*/

        [XmlElement("enableVideoSendKey")]
        public string enableVideoSendKeyString
        {
            get { return _enableVideoSendKey.ToString(); }
            set
            {
                KeysConverter kc = new KeysConverter();
                try
                {
                    _enableVideoSendKey = (Keys)kc.ConvertFromString(value);
                }
                catch (System.NotSupportedException)
                {
                    _enableVideoSendKey = Keys.Control | Keys.Alt | Keys.S;
                }
            }
        }

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
        public int port
        {
            get { return _port; }
            set { _port = value; }
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

        [XmlElement("maxConnections")]
        public int maxConnections
        {
            get { return _maxConnections; }
            set { _maxConnections = value; }
        }

    }

}
