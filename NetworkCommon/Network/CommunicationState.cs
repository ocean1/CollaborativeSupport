using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils.Network.Packets;
using Logger;


namespace CommonUtils.Network
{


    public class IOCompletedEventArgs: EventArgs
    {
        private Packet packet;

        public Packet Packet
        {
            get { return packet; }
        }

        public IOCompletedEventArgs(Packet packet):base()
        {
            this.packet = packet;
        }
    }

    public enum TransmissionStatus
    {
        MsgContent,
        MsgSize
    };

    public enum TransmissionDirection
    {
        Receive,
        Send
    };

    /// <summary>
    /// this class provides a container for the current
    /// state of the communication
    /// </summary>
    public abstract class CommunicationState
    {

        protected byte[] lenBuffer;
        protected byte[] buffer;
        protected int currentPosition;
        public TransmissionStatus transmissionStatus;
        public TransmissionDirection transmissionDirection;

        public event EventHandler<IOCompletedEventArgs> IOCompletedEvent;


        public int RemaningBytes
        {
            get { return Buffer.Length - CurrentPosition; }
        }

        public int CurrentPosition
        {
            get { return currentPosition; }
        }

        public byte[] Buffer
        {
            get
            {
                switch (transmissionStatus)
                {
                    case TransmissionStatus.MsgContent:
                        return buffer;
                    case TransmissionStatus.MsgSize:
                        return lenBuffer;
                    default:
                        return null;
                }
            }

            set
            {
                switch (transmissionStatus)
                {
                    case TransmissionStatus.MsgContent:
                        buffer = value;
                        break;
                    case TransmissionStatus.MsgSize:
                        lenBuffer = value;
                        break;
                }
            }
        }

        public CommunicationState(TransmissionDirection transmissionDirection)
        {
            this.Clear();
            lenBuffer = new byte[PacketComposer.byteLengthSize];
            this.transmissionDirection = transmissionDirection;
        }

        /// <summary>
        /// when doing a transfer set the number of bytes correctly received
        /// here to know if we need to continue with the transfer operation
        /// </summary>
        /// <param name="bytesTransferred">number of bytes transmitted</param>
        public void TransferredBytes(int bytesTransferred)
        {
            if (bytesTransferred < 0)
                throw new ArgumentOutOfRangeException("bytesTransferred");

            currentPosition += bytesTransferred;
            // still not finished to transfer the message
            if (currentPosition < Buffer.Length)
                return;

            switch (transmissionStatus)
            {
                case TransmissionStatus.MsgSize:
                    int length = BitConverter.ToInt32(this.Buffer, 0);
                    if (length < 0) throw new InvalidPacketException("Invalid packet length");
                    currentPosition = 0;
                    buffer = new byte[length];

                    // finished to receive message size now on to message content
                    transmissionStatus = TransmissionStatus.MsgContent;
                    break;

                case TransmissionStatus.MsgContent:
                    try
                    {
                        Packet packet = (transmissionDirection == TransmissionDirection.Receive) ? packet = PacketComposer.Deserialize(buffer) : null;
                        if( IOCompletedEvent != null )
                            IOCompletedEvent(this, new IOCompletedEventArgs(packet));
                    }
                    catch (Exception e)
                    {
                        MyLogger.Instance.AddError(e, logLevel.ERROR);
                    }

                    break;
            }

        }

        /// <summary>
        /// Clear the current status
        /// to start with another transfer
        /// clear the reference to the buffers and
        /// set transmission status to msg size
        /// </summary>
        public void Clear()
        {
            currentPosition = 0;
            //lenBuffer = null;
            buffer = null;
            
            switch (transmissionDirection)
            {
                case TransmissionDirection.Send:
                    transmissionStatus = TransmissionStatus.MsgContent;
                    break;
                default:
                    transmissionStatus = TransmissionStatus.MsgSize;
                    break;
            }
        }

    }

    public sealed class InCommunicationState : CommunicationState
    {
        public InCommunicationState()
            : base(TransmissionDirection.Receive)
        {
        }
    }

    public sealed class OutCommunicationState : CommunicationState
    {
        public OutCommunicationState()
            : base(TransmissionDirection.Send)
        {
        }

        /// <summary>
        /// Set the internal buffer to contain the packet we
        /// want to send
        /// </summary>
        /// <param name="packet">the packet to be sent</param>
        /// <returns>true if packet correctly set into the buffer
        /// false if there was an error during the serialization</returns>
        public bool SetPacketToSend(Packet packet)
        {
            try
            {
                currentPosition = 0;
                transmissionStatus = TransmissionStatus.MsgContent;
                buffer = PacketComposer.Serialize(packet, true);
            }
            catch (Exception e)
            {
                MyLogger.Instance.AddError(e, logLevel.ERROR);
                return false;
            }
            
            return true;
        }

    }

}
