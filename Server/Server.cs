using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using CommonUtils.Network;
using CommonUtils.Network.Packets;
using System.Threading.Tasks;
using Logger;

namespace Server
{

    public sealed class Server : ConnectionManager, IDisposable
    {

        public VideoManager videoManager;

        private readonly object _lock = new object();
        private bool _disposed;

        private ConcurrentDictionary<string, PeerStatus> clientList; // Registered users <username,ClientStatus>
        private Socket serverSocket; // Socket server usato per ascoltare i client
        private AuthenticationMgr authMgr; // authentication manager to manage clients authentication
        private int port;
        private MyLogger logger;
        private int maxConnections;

        const int saltSize = 8;

        public Server(int port, string username, string password, int maxConnections)
            : base()
        {
            logger = MyLogger.Instance;
            clientList = new ConcurrentDictionary<string, PeerStatus>();
            this.port = port;
            authMgr = new AuthenticationMgr(username, password);
            this.maxConnections = maxConnections;

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, this.port);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(100);
        }

        #region Socket Async Callbacks

        public override void Begin()
        {
            serverSocket.BeginAccept(new AsyncCallback(OnAccept), null); // Start accepting client connections
            printMessage("**** Server Online: Accepting new client connections\r\n");
        }

        private void OnAccept(IAsyncResult ar)
        {
            lock (_lock)
            {
                if (_disposed) return; // don't continue if disposed

                PeerStatus cs = null;
                try
                {
                    cs = new PeerStatus(serverSocket.EndAccept(ar));
                    cs.PeerDisconnectedEvent += new EventHandler(cs_PeerDisconnectedEvent);

                    // set the io 
                    UserLogin(cs);

                }
                catch (Exception ex)
                {
                    logger.AddError(ex, logLevel.ERROR);
                    printError("Errore: " + ex.Message);

                    if (cs != null) cs.Dispose();
                }
                finally
                {
                    try
                    {
                        serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                    }
                    catch (Exception ex)
                    {
                        logger.AddError(ex, logLevel.SEVERE_ERROR);
                        printError("An error occurred accepting new client connections!");
                    }
                }
            }
        }

        void cs_PeerDisconnectedEvent(object sender, EventArgs e)
        {
            PeerStatus clientStatus = (PeerStatus)sender;

            UserLogout(clientStatus);
        }

        private void PacketReceivedEventHandler(object sender, IOCompletedEventArgs args)
        {
            AnalyzePacket(args.Packet, (PeerStatus)sender);
        }

        private void AnalyzePacket(Packet packet, PeerStatus clientStatus)
        {

            switch (packet.cmd)
            {
                case Packet.Command.Message:
                    DataPacket dp = (DataPacket)packet;
                    MessageReceived(dp, clientStatus);
                    PacketForward(dp, clientStatus);
                    break;
                case Packet.Command.Clipboard:
                    ClipboardPacket cp = (ClipboardPacket)packet;
                    cp.username = clientStatus.Name; //sanity fix
                    ClipDataReceived(cp);
                    PacketForward(cp, clientStatus);
                    break;
                case Packet.Command.Logout:
                    this.UserLogout(clientStatus);
                    break;
            }

        }


        #endregion

        private void UserLogout(PeerStatus cs)
        {
            try
            {
                if (cs.authenticated)
                {
                    printMessage(String.Format("Client \"{0}\" left the room.\r\n", cs.Name));
                    PeerStatus dummy;
                    clientList.TryRemove(cs.Name, out dummy);

                    DataPacket reply = new DataPacket(Packet.Command.Logout, cs.Name);
                    this.SendAllClients(reply, cs);

                    removeUserFromList(cs.Name);
                }
                cs.Dispose();
                return;
            }
            catch (Exception ex)
            {
                printError("Client socket not found.\r\n");
                logger.AddError(ex, logLevel.ERROR);
                throw new InvalidPacketException(string.Empty, null);
            }

        }

        private void UserLogin(PeerStatus cs)
        {
            int timeOut = 3000;
            // generate the salt for the specific client
            cs.salt = AuthenticationMgr.GenerateSalt(saltSize);

            // generate authentication request and send to client 
            cs.SendSync(new AuthenticationRequest(cs.salt), timeOut);

            Packet resp = cs.ReceiveSync(timeOut);
            CollaborationError err = CollaborationError.AuthFailed;

            if (resp != null && resp.cmd == Packet.Command.Authenticate)
            {
                AuthenticationResponse authResp = (AuthenticationResponse)resp; //of course is a loginpacket

                if (clientList.Count() >= maxConnections)
                {
                    // Can't connect max users reached
                    err = CollaborationError.MaxUsersReached;
                }
                else if (authMgr.VerifyResponse(authResp, cs.salt))
                {
                    // TODO: qui controlla che non ci siano utenti con lo stesso nome altrimenti invia un nome rand()
                    cs.authenticated = true;
                    cs.Name = authResp.username;

                    if (cs.Name == authMgr.username || clientList.TryAdd(cs.Name, cs) == false)
                    {
                        // if TryAdd return false we have to kill the connection telling the nickname is already used or assign a available nick
                        err = CollaborationError.UserNameTaken;
                    }
                    else
                    {

                        addUserToList(authResp.username);
                        printMessage(authResp.username + " connected.");

                        cs.PacketReceivedEvent += PacketReceivedEventHandler;
                        cs.StartReceive(false);
                        //invia l'OK al client
                        Packet ok = new DataPacket(Packet.Command.OK, null);
                        cs.Send(ok);
                        // invia la lista di utenti al client appena connesso
                        List<string> userlist = new List<string>();
                        userlist.Add(authMgr.username);
                        userlist.AddRange(clientList.Keys);

                        Packet userslist = new UserList(userlist.ToArray());
                        cs.Send(userslist);
                        string[] list = new string[1];
                        list[0] = authResp.username;
                        // quindi ritorna un messaggio contente le info del nuovo utente connesso da inviare a tutti gli altri
                        Packet loggedin = new UserList(list);
                        this.SendAllClients(loggedin, cs);

                        // send the new image if videoManager is active
                        if (videoManager != null) videoManager.ForceSendNewImage();
                    }
                }
                else
                {
                    err = CollaborationError.AuthFailed;
                }

            }

            if (err != CollaborationError.None) cs.Send(new ErrorPacket(err));

        }

        public override void SendClipData(string description, object data, string format)
        {
            ClipboardPacket cp = new ClipboardPacket(authMgr.username, description, data, format);
            base.ClipDataReceived(cp);
            this.SendAllClients(cp, null);
        }

        public override void SendMessage(string msg)
        {
            DataPacket dp = new DataPacket(Packet.Command.Message, authMgr.username, msg);
            base.MessageReceived(dp, null);
            this.SendAllClients(dp, null);
        }

        private void PacketForward(Packet msgReceived, PeerStatus cs)
        {
            Socket clientSocket = cs.socket;
            Packet dp_out = msgReceived;

            if (msgReceived.cmd == Packet.Command.Message)
            {
                ((DataPacket)dp_out).username = cs.Name; // the name is always going to be the one specified in the clientstatus
            }
            if (msgReceived.cmd == Packet.Command.Clipboard)
            {
                ((ClipboardPacket)dp_out).username = cs.Name; // the name is always going to be the one specified in the clientstatus
            }

            SendAllClients(dp_out, cs);
        }

        public void SendAllClients(Packet packet, PeerStatus cs)
        {
            //  invio il messaggio a tutti gli utenti online ( only cmd = message || cmd = logout )
            foreach (KeyValuePair<string, PeerStatus> clientInfo in clientList)
            {

                if (cs == null)    // if null it's the server sending a packet to everyone
                {
                    // reply.strName = clientInfo.Value.strName;
                    clientInfo.Value.Send(packet);
                }
                else if (cs.socket != clientInfo.Value.socket)
                {
                    clientInfo.Value.Send(packet);
                }
            }
        }


        private bool DisconnectClient(PeerStatus cs)
        {

            //ClientStatus cs;
            //bool result = clientList.TryRemove(clientSocket, out cs);
            if (cs.authenticated)
            {
                printMessage(cs.Name + " has disconnected\n");
            }
            else
            {
                logger.AddError("An unauthenticated user disconnected, socket info:" + cs.socket.ToString(), logLevel.WARNING);
            }

            PeerStatus dummy;
            if (clientList.TryRemove(cs.Name, out dummy)) cs.Dispose();
            //RemoveUser(cs.Name);

            // should client socket be still connected disconnect it
            /*if (false)
            {
                cs.clientSocket.Shutdown(SocketShutdown.Both);
                cs.clientSocket.Disconnect(false);
            }*/
            return true;
        }


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
                        serverSocket.Close();

                        foreach (PeerStatus cs in clientList.Values)
                        {
                            //cs.socket.Shutdown(SocketShutdown.Both);
                            //cs.socket.Close();
                            cs.Dispose();
                        }

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

        ~Server()
        {
            Dispose(false);
        }
        #endregion

    }
}
