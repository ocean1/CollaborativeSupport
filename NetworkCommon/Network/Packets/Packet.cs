namespace CommonUtils.Network.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public abstract class Packet
    {
        public enum Command
        {
            Null,           // No command
            Authenticate,   // Login nel server
            Logout,         // Logout dal server
            Message,        // Invio un messaggio
            Error,          // Messaggio di errore
            Video,          // packet with video content
            Clipboard,      // clipboard data
            screen_off,     // Schermo off
            List,           // Lista utenti presenti nella chat
            OK,             // Registrazione avvenuta con successo
            UserPresent,    // Username già presente
            WrongPassword,  // Password errata
            Quit,           // Quit Server
            BigPacket,      // next packets=fragments until full packet is received
            MousePos        // if needed we send only the mouse position
        }

        public Command cmd;  // Command type (login, logout, send message, etc.)

        public Packet()
        {
            this.cmd = Command.Null;
        }

        public Packet(Command cmd)
        {
            this.cmd = cmd;
        }

    }
}
