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
            if(DateTime.Now.Hour < 4 || DateTime.Now.Hour > 12)
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Evening."));
            else
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Morning."));
        }

        public override string Help()
        {
            return "Sends the current <?>ning";
        }
    }
}
