using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Logger;
using CommonUtils.Network.Packets;
using System.Threading.Tasks;

namespace CommonUtils.Network
{

    public class PeerStatus : IDisposable
    {

        //public delegate void PacketReceived(object sender, EventArgs e);
        public event EventHandler PeerDisconnectedEvent;
        public event EventHandler<IOCompletedEventArgs> PacketReceivedEvent;
        //public event PacketReceived PacketReceivedEvent;

        public Socket socket;
        public string Name; // Username utente

        public InCommunicationState InState;
        public OutCommunicationState OutState;

        public bool authenticated; // defaults to "false" = non authenticated
        public byte[] salt;


        private readonly object _lock_receive = new object();
        private readonly object _lock_send = new object();
        private bool _disposed;

        // this queue contains outgoing messages since we shouldn't start more than one send operation
        // or data would be mixed up on the socket and we want to send/receive in order
        private Queue<Packet> outgoingMessagesQueue;

        private int messagesToReceiveNum = 0;

        public PeerStatus(Socket socket)
        {
            InState = new InCommunicationState();
            OutState = new OutCommunicationState();

            InState.IOCompletedEvent += IOCompletedEventHandler;
            OutState.IOCompletedEvent += IOCompletedEventHandler;

            this.socket = socket;
            outgoingMessagesQueue = new Queue<Packet>();
        }

        public void IOCompletedEventHandler(object sender, IOCompletedEventArgs args)
        {
            CommunicationState ioState = (CommunicationState)sender;
            if( ioState.transmissionDirection == TransmissionDirection.Receive && PacketReceivedEvent!=null )
                PacketReceivedEvent(this, args);   // pass the event to the sup. level
        }

        public void StartReceive(bool singleMessage)
        {
            lock (this._lock_receive)
            {
                if (this._disposed) return; // don't continue if disposed
                messagesToReceiveNum = (singleMessage) ? 1 : -1;
                InState.Clear();
                socket.BeginReceive(this.InState.Buffer, 0, this.InState.RemaningBytes, SocketFlags.None, new AsyncCallback(OnReceive), this);
            }
        }

        private static void OnReceive(IAsyncResult ar)
        {
            PeerStatus cs = (PeerStatus)ar.AsyncState;
            Socket clientSocket = cs.socket;
            lock (cs._lock_receive)
            {
                if (cs._disposed) return; // don't continue if disposed

                try
                {

                    // make sure the client is still connected and that socket hasn't been shut down
                    int bytesReceived = clientSocket.EndReceive(ar);
                    if (bytesReceived > 0)
                    {
                        cs.InState.TransferredBytes(bytesReceived);

                        if (cs.InState.RemaningBytes > 0)
                        {
                            clientSocket.BeginReceive(cs.InState.Buffer,
                                cs.InState.CurrentPosition,
                                cs.InState.RemaningBytes,
                                SocketFlags.None,
                                new AsyncCallback(OnReceive),
                                cs);
                        }
                        else
                        {
                            // we don't need to start receiving if we didn't receive
                            // the whole packet
                            if (cs.messagesToReceiveNum == -1 || (--cs.messagesToReceiveNum > 0))
                            {
                                cs.StartReceive(false);
                            }
                        }
                    }
                    else
                    {
                        // client closed connection
                        //DisconnectClient(cs);
                        if( cs.PeerDisconnectedEvent != null )
                            cs.PeerDisconnectedEvent(cs, new EventArgs());
                        // signal that client closed connection
                    }

                    //always begin a new receive waiting for the next packet
                }
                catch (Exception ex)
                {
                    // DisconnectClient(cs);
                    // printError("Errore: " + ex.Message);
                    MyLogger.Instance.AddError(ex, logLevel.ERROR);
                }
            }
        }


        public void Send(Packet packet)
        {
            lock (this._lock_send)
            {
                if (this._disposed) return; // don't continue if disposed

                outgoingMessagesQueue.Enqueue(packet);
                if (outgoingMessagesQueue.Count() == 1)
                {
                    try
                    {
                        OutState.Clear();
                        OutState.SetPacketToSend(packet);

                        socket.BeginSend(
                            OutState.Buffer,
                            0,
                            OutState.RemaningBytes,
                            SocketFlags.None,
                            new AsyncCallback(OnSend),
                            this);
                    }
                    catch (Exception ex)
                    {
                        MyLogger.Instance.AddError(ex, logLevel.ERROR);
                    }
                }
            }
        }

        private static void OnSend(IAsyncResult ar)
        {
            PeerStatus cs = (PeerStatus)ar.AsyncState;
            Socket clientSocket = cs.socket;

            lock (cs._lock_send)
            {
                if (cs._disposed) return; // don't continue if disposed

                try
                {
                    int bytesSent = clientSocket.EndSend(ar);
                    if (bytesSent > 0)
                    {
                        cs.OutState.TransferredBytes(bytesSent);

                        if (cs.OutState.RemaningBytes > 0)
                        {
                            clientSocket.BeginSend(cs.InState.Buffer,
                                cs.OutState.CurrentPosition,
                                cs.OutState.RemaningBytes,
                                SocketFlags.None,
                                new AsyncCallback(OnSend),
                                cs);
                        }
                        else
                        {
                            // jump the packet this has already been sent!
                            cs.outgoingMessagesQueue.Dequeue();
                            if (cs.outgoingMessagesQueue.Count > 0)
                            {
                                
                                Packet packet = cs.outgoingMessagesQueue.First();
                                // get the first packet and send it!
                                cs.OutState.Clear();
                                cs.OutState.SetPacketToSend(packet);
                                clientSocket.BeginSend(
                                    cs.OutState.Buffer,
                                    0,
                                    cs.OutState.RemaningBytes,
                                    SocketFlags.None,
                                    new AsyncCallback(OnSend),
                                    cs);
                            }

                        }

                    }
                    else
                    {
                        // client closed connection
                        //DisconnectClient(cs);
                        if (cs.PeerDisconnectedEvent != null)
                            cs.PeerDisconnectedEvent(cs, new EventArgs());
                        // signal that client closed connection
                    }
                }
                catch (Exception e)
                {
                    MyLogger.Instance.AddError(e, logLevel.ERROR);
                }

            }

        }

        public Packet ReceiveSync(int timeOut)
        {
            int tempTO;
            Packet packet = null;
            tempTO = socket.ReceiveTimeout;
            InState.Clear();

            InState.IOCompletedEvent+= (s,arg) => { packet = arg.Packet; };

            do{
                int bytesRead = socket.Receive(
                    InState.Buffer,
                    InState.CurrentPosition,
                    InState.RemaningBytes,
                    SocketFlags.None);
                InState.TransferredBytes(bytesRead);
            }while( InState.RemaningBytes > 0 );

            // we received the packet now decode it
            return packet;

        }

        public void SendSync(Packet packet, int timeOut)
        {
            int tempTO;
            tempTO = socket.SendTimeout;
            OutState.SetPacketToSend(packet);

            do{
                int bytesSent = socket.Send(
                    OutState.Buffer,
                    OutState.CurrentPosition,
                    OutState.RemaningBytes,
                    SocketFlags.None);
                OutState.TransferredBytes(bytesSent);
            } while (OutState.RemaningBytes > 0);
            
            socket.SendTimeout = timeOut;
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true); // dispose managed and unmanaged resources alike!
            GC.SuppressFinalize(this); // avoid to execute finalization of this class from happening two times
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
            lock (_lock_receive)
            lock(_lock_send)
            {
                if (_disposed == false)
                {
                    // If disposing equals true, dispose all managed 
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // clean up managed resources here
                        outgoingMessagesQueue.Clear(); // clean up content of outgoing message queue
                        // clear references
                        outgoingMessagesQueue = null;
                        InState = null;
                        OutState = null;
                    }
                    // clean up unmanaged resources here

                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        socket = null;
                    }
                    catch (SocketException se)
                    {
                        MyLogger.Instance.AddError(se, logLevel.ERROR);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        // should not be disposed until now...
                        MyLogger.Instance.AddError(ex, logLevel.ERROR);
                    }


                }
                _disposed = true;
            }
        }

        ~PeerStatus()
        {
            Dispose(false);
        }
        #endregion

    }
}
