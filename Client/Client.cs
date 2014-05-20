using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CommonUtils.Network;
using CommonUtils.Network.Packets;
using Logger;

namespace Client
{
    public class Client : ConnectionManager
    {

        private readonly object _lock = new object();
        private bool _disposed;

        private IPEndPoint remoteEP;

        private PeerStatus status; // of course the client status can be rapresented by a
        // PeerStatus class instance

        public event EventHandler PeerDisconnectedEvent;

        private ManualResetEventSlim connectDone;
        private AuthenticationMgr authMgr;

        MyLogger logger;

        public Client(string serverUri, string username, string password, PrintErrorDelegate printError)
            : base()
        {
            authMgr = new AuthenticationMgr(username, password);
            this.printError = printError;

            logger = MyLogger.Instance;
            //set the default delegate in case it isn't set by anyone
            try
            {
                remoteEP = NetworkUtils.ParseEndPoint(serverUri);

            }
            catch (Exception e)
            {
                printError(e.Message);
                throw;
            }

        }

        public override void Begin()
        {
            // if packet processed begin to receive again on the client socket
            // and start receiving only the length then after this we will get the rest of the packet 
            status.PacketReceivedEvent += PacketReceivedEventHandler;
            status.StartReceive(false);
        }

        public bool Connect()
        {

            try
            {
                using (connectDone = new ManualResetEventSlim())
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    status = new PeerStatus(socket);
                    status.PeerDisconnectedEvent += ServerDisconnectedEventHandler;

                    status.socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), status);

                    connectDone.Wait(); // aspetta che la connessione sia terminata, viene segnalato dall'async callback
                }

                return status.authenticated;
            }
            catch (Exception e)
            {
                printError(e.Message);
                logger.AddError(e, logLevel.ERROR);

                // retrow the exception? we should really implement a fucking event :P
                throw;
            }
        }

        private void ServerDisconnectedEventHandler(object sender, EventArgs e)
        {
            printMessage("Server closed the connection!");
            printMessage("Stopping the client!");
            status.Dispose();

            //now tell it to the mainform to disable everything!            
            PeerDisconnectedEvent(this, new EventArgs());

        }

        private void PacketReceivedEventHandler(object sender, IOCompletedEventArgs args)
        {

            Packet packet = args.Packet;

            #region Message Command Switch
            switch (packet.cmd)
            {
                case Packet.Command.Message:
                    this.MessageReceived((DataPacket)packet, null);
                    break;

                case Packet.Command.Logout:
                    this.UserLogout((DataPacket)packet);
                    return;

                case Packet.Command.Video:
                    VideoPacket vp = (VideoPacket)packet;
                    if (showVideo != null)
                        this.showVideo(vp);
                    break;

                case Packet.Command.Clipboard:
                    ClipboardPacket cp = (ClipboardPacket)packet;
                    showClipData(cp.username, cp.description, cp.clipBoardData, cp.format);
                    break;

                case Packet.Command.List:
                    UserList users = (UserList)packet;
                    foreach (string user in users) addUserToList(user);
                    break;
            }
            #endregion

        }

        private void UserLogout(DataPacket dataPacket)
        {
            string name = dataPacket.Message;
            
            printMessage(String.Format("Client \"{0}\" left the room.\r\n", name));
            removeUserFromList(name);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                PeerStatus clientStatus = (PeerStatus)ar.AsyncState; // Retrieve the socket from the state object.
                Socket clientSocket = clientStatus.socket;

                clientSocket.EndConnect(ar); // Complete the connection.

                CollaborationError ce = Authenticate();
                
                // if no errors on connection we have correctly authenticated
                // and set the auth. status to true
                // else print the error and signal we have
                // finished the connection procedure
                if (ce == CollaborationError.None)
                    status.authenticated = true;
                else
                    printMessage(ErrorPacket.getErrorString(ce));

            }
            catch (Exception ex)
            {
                printError(ex.Message);
                MyLogger.Instance.AddError(ex, logLevel.ERROR);
                status.authenticated = false;
            }
            finally
            {
                connectDone.Set();
            }
        }

        private CollaborationError Authenticate()
        {
            int timeOut = 3000;

            Packet authReq = status.ReceiveSync(timeOut);
            if (authReq != null && authReq.cmd == Packet.Command.Authenticate)
            {
                authMgr.salt = ((AuthenticationRequest)authReq).salt; // set the salt to the one received
                AuthenticationResponse ar = authMgr.GenerateResponse(); // generate the response
                status.SendSync(ar, timeOut); // send it

                Packet response = status.ReceiveSync(timeOut);

                if (response != null)
                {
                    switch (response.cmd)
                    {
                        case Packet.Command.OK:
                            return CollaborationError.None;
                        default:
                            return ((ErrorPacket)response).error;
                    }
                }
            }

            return CollaborationError.AuthFailed;
        }


        public override void SendMessage(string msg)
        {
            DataPacket dp = new DataPacket(Packet.Command.Message, authMgr.username, msg);
            base.MessageReceived(dp, null);
            status.Send(dp);
        }


        public override void SendClipData(string description, object data, string format)
        {
            ClipboardPacket cp = new ClipboardPacket(authMgr.username, description, data, format);
            ClipDataReceived(cp);
            status.Send(cp);
        }


        #region client connected event
        public delegate void ClientConnectedEventHandler(object sender, NewClientEventArgs e);


        // hold event arguments
        public class ClientConnectedEventArgs : System.EventArgs
        {
            private bool connected;

            public ClientConnectedEventArgs(bool connected)
            {
                this.connected = connected;
            }

            public bool IsConnected
            {
                get
                {
                    return this.connected;
                }
            }
        }
        #endregion


        #region IDisposable implementation

        public override void Dispose()
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
        protected override void Dispose(bool disposing)
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
                    }

                    // clean up unmanaged resources here
                    try
                    {
                        //serverSocket.Shutdown(SocketShutdown.Both);
                        status.Dispose();

                        base.Dispose(true);
                    }
                    catch (SocketException se)
                    {
                        printError(se.Message);
                        logger.AddError(se, logLevel.ERROR);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        // should not be disposed until now...
                        printError(ex.Message);
                        logger.AddError(ex, logLevel.ERROR);
                    }

                }
                _disposed = true;
            }
        }

        ~Client()
        {
            Dispose(false);
        }
        #endregion


    }
}
