using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    public class IRCMessage
    {
        public string MessageTarget { get; private set; }
        public string MessageText { get; private set; }
        public string MessageFrom { get; private set; }
        public string ServerCmd { get; private set; }

        public IRCMessage(string command, string target, string text)
        {
            ServerCmd = command;
            MessageTarget = target;
            MessageText = text;
            MessageFrom = "";
        }

        public IRCMessage(string command, string target, string text, string from)
        {
            ServerCmd = command;
            MessageTarget = target;
            MessageText = text;
            MessageFrom = from;
        }

        public IRCMessage(string command, string msg)
        {
            MessageText = msg;
            ServerCmd = command;
            MessageFrom = "";
            MessageTarget = "";
        }

        public byte[] ToByteArray()
        {
            byte[] output;

            if (MessageTarget != "")
            {
                output = System.Text.Encoding.UTF8.GetBytes(ServerCmd + " " + MessageTarget + " :" + MessageText);
            }
            else
            {
                output = System.Text.Encoding.UTF8.GetBytes(ServerCmd + " :" + MessageText);
            }

            return output;
        }
    }
}
