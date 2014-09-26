using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class EventTimer : Module
    {
        public class Time
        {
            public Time(int hr, int min)
            {
                hour = hr;
                minute = min;
            }

            public int hour { get; set; }
            public int minute { get; set; }
        }
        private string eventName;
        private Time eventTime;

        public EventTimer(IRC ircNet, Time time, string name) : base(ircNet)
        {
            eventTime = time;
            eventName = name;
        }

        public override void Start() { }

        public override void Stop() { }

        public override void Save() { }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            string timeUntil = "";

            DateTime time = DateTime.Now;
            if (time.Hour > eventTime.hour || (time.Hour >= eventTime.hour && time.Minute > eventTime.minute))
            {
                time = new DateTime(time.Year, time.Month, time.Day + 1, eventTime.hour, eventTime.minute, 0, 0, DateTimeKind.Unspecified);
            }
            else
            {
                time = new DateTime(time.Year, time.Month, time.Day, eventTime.hour, eventTime.minute, 0, 0, DateTimeKind.Unspecified);
            }
            TimeSpan ts = time - DateTime.Now;

            timeUntil += ts.Hours + "hours, " + ts.Minutes + " minutes.";

            string msg = "Time until " + eventName.Split('$')[1] + ": " + timeUntil;
            IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg); 
            irc.Send(response);
        }

        public override string Help()
        {
            return "Prints the time until " + eventTime.hour + ":" + eventTime.minute + ".";
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
            if (!command[2].StartsWith("$"))
                command[2] = "$" + command[2];
            DateTime eventTime;
            try
            {
                DateTime.TryParse(command[3], out eventTime);
            }
            catch
            {
                string msg = "Could not parse input \"" + command[3].Trim() + "\".";
                IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg);
                irc.Send(response);
                return;
            }

            if(bot.IsModuleNameReserved(command[2]))
            {
                string msg = "\"" + command[2].Trim() + "\" is reserved.";
                IRCMessage response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg);
                irc.Send(response);
                return;
            }



            EventTimer t = new EventTimer(irc, new EventTimer.Time(eventTime.Hour, eventTime.Minute), command[1]);
            bot.AddModule(t, command[2]);
        }

        public override string Help()
        {
            return "$timer <timer name> <timer time>   Add a timer for your user at a given time, which can " +
                   "later be queried using $<timer name>. Some names are reserved.";
        }
    }
}
