using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class Bot
    {
        private IRC irc;
        List<string> adminList;
        Hashtable modules = new Hashtable();

        public Bot(string[] args)
        {
            irc = new IRC("localhost", "KentIRC", "GwenDev", "Simon Moore's C# IRC Bot, v2", 6667);
            irc.IdentNoAuthEvent += new IRC.IdentNoAuthEventHandler(irc_IdentNoAuthEvent);
            irc.MessageEvent += new IRC.MessageEventHandler(irc_MessageEvent);


            adminList = new List<string>();
            adminList.Add("graymalkin");
        }

        public IRC GetIRC() { return irc; }

        public void AddModule(Module m, string command)
        {
            modules.Add(":" + command, m);
        }

        void irc_MessageEvent(object sender, Events.MessageReceivedEventArgs e)
        {
            string[] cmd = e.message.messageText.Split(' ');
            if (cmd.Length > 1 && modules.ContainsKey(cmd[1]))
            {
                Module m = (Module)modules[cmd[1]];
                m.InterpretCommand(cmd, e);
            }

            if (e.message.messageText.StartsWith(" :$join"))
            {
                if (!adminList.Contains(e.message.messageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                    irc.join(cmd[i]);
            }

            if (e.message.messageText.StartsWith(" :$leave"))
            {
                if (!adminList.Contains(e.message.messageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                    irc.part(cmd[i]);
            }

            if (e.message.messageText.StartsWith(" :$quit"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.message.messageFrom + " attempted " + e.message.messageText.Substring(2));
                if (!adminList.Contains(e.message.messageFrom))
                    return;

                string reason = "";

                if (cmd.Length > 2)
                {
                    for (int i = 2; i < cmd.Length; i++)
                        reason += cmd[i].Replace("\r\n", " ") + " ";
                }
                else
                    reason = "No reason given";

                irc.quit(reason);
            }

            if (e.message.messageText.StartsWith(" :$add_admin"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.message.messageFrom + " attempted " + e.message.messageText.Substring(2));
                if (e.message.messageFrom != "graymalkin")
                    return;

                for (int i = 2; i < cmd.Length; i++)
                {
                    adminList.Add(cmd[i].Trim());
                    Output.Write("ADMIN", ConsoleColor.Red, cmd[i].Trim() + " added to admins");

                }
            }

            if (e.message.messageText.StartsWith(" :$remove_admin"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.message.messageFrom + " attempted " + e.message.messageText.Substring(2));
                if (!adminList.Contains(e.message.messageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                {
                    if (cmd[i].Trim() != "graymalkin")
                    {
                        adminList.Remove(cmd[i].Trim());
                        Output.Write("ADMIN", ConsoleColor.Red, cmd[i].Trim() + " removed from admins");
                    }
                }
            }

            if (e.message.messageText.StartsWith(" :$msgBoard"))
            {
                string ms = "";
                for (int i = 2; i < cmd.Length; i++)
                {
                    ms += " " + cmd[i];
                }


                Output.Write("MsgBoard", ConsoleColor.Green, ms);
            }

            if (e.message.messageText.StartsWith(" :$compliment_zebr"))
                irc.send(new Message("PRIVMSG", e.message.messageTarget, getCompliment()));

            if (e.message.messageText.StartsWith(" :$help") && e.message.messageText.Split(' ').Length <= 2)
                irc.send(new Message("PRIVMSG", e.message.messageTarget, @"Help can be found at http://graymalk.in/ircbot/"));

            if (e.message.messageText.StartsWith(" :$help") && e.message.messageText.Split(' ').Length > 2)
                GetHelp(e);

            if (e.message.messageText.StartsWith(" :$modules"))
                PrintModules(e);

            if (e.message.messageFrom == "zebr" && (e.message.messageText.Contains("Gwen: ") || e.message.messageText.Contains("Gwen ")))
                irc.send(new Message("PRIVMSG", e.message.messageTarget, "zebr: <3"));


        }

        public string getCompliment()
        {
            string[] compliments = { "You're looking lovely today, zebr", "zebr: You're cute :$", "Hey zebr, you're the best!",
                                     "I'd love to spend some more time with zebr", "I have a bit of a crush on zebr, he's so cool",
                                     "zebr: you're funny :$", "zebr, you don't get drunk - you get superhuman!", 
                                     "zebr has the best jokes", "This one time, I was at a bar, and I said 'this would be better if zebr were here'",
                                     "I wish some people were more like zebr", "zebr would make the best Dad!",
                                     "You know who's cool? zebr is cool!", "<3 you, zebr", "Some people are boring, zebr isn't one of them."};
            int number = new Random().Next(compliments.Length);
            return compliments[number];
        }

        void irc_IdentNoAuthEvent(object sender, Events.IdentAuthNoResponseEventArgs e)
        {
            irc.login();
        }

        private void PrintModules(Events.MessageReceivedEventArgs e)
        {
            string output = "";
            foreach(string s in modules.Keys)
            {
                output += s + " ";
            }
            output = output.Trim().Replace(":", "");
            irc.send(new Message("PRIVMSG", e.message.messageTarget, output));
        }

        private void GetHelp(Events.MessageReceivedEventArgs e)
        { 
            string m = ":" + e.message.messageText.Split(' ')[2];
            if(modules.ContainsKey(m))
            {
                irc.send(new Message("PRIVMSG", e.message.messageTarget, ((Module)modules[m]).Help()));
            }
        }
    }
}
