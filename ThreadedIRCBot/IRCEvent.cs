using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message message { get; private set; }

        public MessageReceivedEventArgs(Message m)
        {
            message = m;
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
