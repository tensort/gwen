using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class Roll : Module
    {
        Random r;
        public Roll(IRC ircNet) : base(ircNet) 
        {
            r = new Random();
        }

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
            if(command.Length <= 2)
            {
                irc.send(new Message("PRIVMSG", e.message.messageTarget, "Roll of " + r.Next(100)));
                return;
            }

            string output = "";
            int total = 0;
            foreach(string si in command)
            {
                string s = si.ToLower();
                if(s.Contains("d") && s.Split('d').Length > 1)
                {
                    try
                    {
                        int number = int.Parse(s.Split('d')[0]);
                        int max = int.Parse(s.Split('d')[1]);
                        for (int i = 0; i < number && i < 5; i++)
                        {
                            int b = r.Next(max);
                            output += b + " ";
                            total += b;
                        }
                    }
                    catch { }
                }
            }

            irc.send(new Message("PRIVMSG", e.message.messageTarget, "Roll of " + output.Trim() + ", total of " + total));
        }

        public override string Help()
        {
            return "Rolls a dice!";
        }
    }
}
