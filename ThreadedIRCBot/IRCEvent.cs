using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public IRCMessage Message { get; private set; }

        public MessageReceivedEventArgs(IRCMessage m)
        {
            Message = m;
        }
    }

    public class IdentAuthNoResponseEventArgs : EventArgs 
    {
        public IdentAuthNoResponseEventArgs()
        {
            // Nothing to do
        }
    }
}
