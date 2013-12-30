using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class MessageBoard : Module
    {
        public MessageBoard(IRC ircNet) : base(ircNet) { }

        string Message = "";
        public override void Start()
        {
            
        }

        public override void Stop()
        {
        }

        public override void Save()
        {
        }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            string s = "";

            if (command.Length == 2)
            {
                irc.send(new Message("PRIVMSG", e.message.messageTarget, "Message Board Set to: \"" + Message + "\""));
                return;
            }

            for (int i = 2; i < command.Length; i++)
                s += command[i] + " ";

            Message = s.Trim();
            irc.send(new Message("PRIVMSG", e.message.messageTarget, "Message Board Set to: \"" + Message + "\""));
        }

        public override string Help()
        {
            return "Get or set the shed's message board text";
        }
    }
}
