using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    public class Message
    {
        public string messageTarget { get; private set; }
        public string messageText { get; private set; }
        public string messageFrom { get; private set; }
        public string servercmd { get; private set; }

        public Message(string command, string target, string text)
        {
            servercmd = command;
            messageTarget = target;
            messageText = text;
            messageFrom = "";
        }

        public Message(string command, string target, string text, string from)
        {
            servercmd = command;
            messageTarget = target;
            messageText = text;
            messageFrom = from;
        }

        public Message(string command, string msg)
        {
            messageText = msg;
            servercmd = command;
            messageFrom = "";
            messageTarget = "";
        }

        public byte[] toByteArray()
        {
            byte[] output;

            if (messageTarget != "")
            {
                output = System.Text.Encoding.UTF8.GetBytes(servercmd + " " + messageTarget + " :" + messageText);
            }
            else
            {
                output = System.Text.Encoding.UTF8.GetBytes(servercmd + " :" + messageText);
            }

            return output;
        }
    }
}
