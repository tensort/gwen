using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class Ning : Module
    {
        public Ning(IRC ircNet) : base(ircNet) { }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
	    string msg = "";
	    if (command.Length > 2)
            {
	        msg += " ";
                for (int i = 2; i < command.Length; i++)
                    msg += command[i].Replace("\r\n", " ") + " ";
            }
	    msg = msg.TrimEnd();

            if(DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 17)
            {
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Afternooning" + msg + "."));
                return;
            }
            if(DateTime.Now.Hour > 17)
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Evening" + msg + "."));
            else
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Morning" + msg + "."));
        }

        public override string Help()
        {
            return "Sends the current <?>ning";
        }
    }
}
