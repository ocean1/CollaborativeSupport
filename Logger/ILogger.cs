using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public interface ILogger : IDisposable
    {
        void ProcessMessage(string Message);
        void ProcessError(string Error, logLevel severity);
        // void ProcessUserConnection(string Username); // non serve sarà inserito nei messaggi userxxx connected ;)
    }

}
