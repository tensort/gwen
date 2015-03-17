using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class BotSnack : Module
    {
        public BotSnack(IRC ircNet) : base(ircNet) { }

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
            irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, ":D"));
        }

        public override string Help()
        {
            return "Eats a tastey snack!";
        }
    }
}
