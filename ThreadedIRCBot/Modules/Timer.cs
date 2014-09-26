using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class EventTimer : Module
    {
        private DateTime eventTime;
        private string eventName;

        public EventTimer(IRC ircNet, DateTime time, string name) : base(ircNet)
        {
            eventTime = time;
            eventName = name;
        }

        public override void Start() { }

        public override void Stop() { }

        public override void Save() { }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            string msg = "test";
            IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg); 
            irc.Send(response);
        }

        public override string Help()
        {
            return "Prints the time until " + eventTime.TimeOfDay.ToString("hh:mm") + ".";
        }
    }

    class Timer : Module
    {
        List<EventTimer> timerList = new List<EventTimer>();
        Bot bot;
        public Timer(IRC ircNet, Bot b) : base(ircNet)
        {
            bot = b;
        }

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
            DateTime eventTime;
            try
            {
                DateTime.TryParse(command[2], out eventTime);
            }
            catch
            {
                string msg = "Could not parse input \"" + command[2].Trim() + "\".";
                IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg);
                irc.Send(response);
                return;
            }

            if(bot.IsModuleNameReserved(command[1]))
            {
                string msg = "\"" + command[1].Trim() + "\" is reserved.";
                IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg);
                irc.Send(response);
                return;
            }

            EventTimer t = new EventTimer(irc, eventTime, command[1]);
            bot.AddModule(t, command[1]);
        }

        public override string Help()
        {
            return "$timer <timer name> <timer time>   Add a timer for your user at a given time, which can " +
                   "later be queried using $<timer name>. Some names are reserved.";
        }
    }
}
