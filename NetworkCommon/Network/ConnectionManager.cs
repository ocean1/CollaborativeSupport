using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;
using CommonUtils.Video;
using CommonUtils.Network;
using CommonUtils.Network.Packets;
using System.Net.Sockets;
using Logger;

namespace CommonUtils.Network
{
    /// <summary>
    /// this abstract class defines some behaviour that's common for the server and client classes
    /// that inherits from ConnectionManager and implement the abstract methods
    /// </summary>
    public abstract class ConnectionManager : IDisposable
    {
        public delegate void AddUserToListDelegate(string s);
        public delegate void RemoveUserFromList(string s);
        public delegate void PrintMessageDelegate(string s);
        public delegate void PrintErrorDelegate(string s);
        public delegate void ShowVideoDelegate(VideoPacket vp);
        public delegate void ShowClipDataDelegate(string username, string description, object data, string format);

        public AddUserToListDelegate addUserToList;
        public RemoveUserFromList removeUserFromList;
        public PrintMessageDelegate printMessage;
        public PrintErrorDelegate printError;
        public ShowVideoDelegate showVideo;
        public ShowClipDataDelegate showClipData;

        public abstract void SendMessage(string msg);
        public abstract void SendClipData(string description, object data, string format);

        //in case of the server we need to override begin to start accepting connections
        //for the client we need to override to start receiving packets after authentication is succesfull
        public abstract void Begin();

        private MyLogger logger;

        protected ConnectionManager()
        {
            logger = MyLogger.Instance;
        }

        protected void MessageReceived(DataPacket dp, PeerStatus cs)
        {
            if (cs != null)
            {
                // for the server use the cs.Name don't trust name sent in the packet!
                MessageReceived(cs.Name,dp.Message);
            }
            else
            {
                // if cs is null it's used internally to simulate the receiving of a Message in the server
                // or when receiving a message in the client
                MessageReceived(dp.username, dp.Message);
            }
        }

        public void MessageReceived(string username, string message)
        {
            printMessage(username + ": " + message);
        }

        protected void ClipDataReceived(ClipboardPacket cp)
        {
            showClipData(cp.username, cp.description, cp.clipBoardData, cp.format);
        }

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
            }
            // clean up unmanaged resources here
        }

        ~ConnectionManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
