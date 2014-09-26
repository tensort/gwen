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
        private const string mainAdmin = "graymalkin";

        public Bot(string[] args)
        {
            irc = new IRC("localhost", "KentIRC", "GwenDev", "Simon Moore's C# IRC Bot, v2", 6667);
            irc.IdentNoAuthEvent += new IRC.IdentNoAuthEventHandler(irc_IdentNoAuthEvent);  // Work around
            irc.MessageEvent += new IRC.MessageEventHandler(irc_MessageEvent);              // Kinda essential.


            adminList = new List<string>();
            adminList.Add(mainAdmin);
        }

        public void AddModule(Module m, string command)
        {
            modules.Add(":" + command, m);
        }

        public bool IsModuleNameReserved(string command)
        {
            return (modules[":" + command] != null) || !(modules[":" + command] is EventTimer);
        }

        void irc_MessageEvent(object sender, Events.MessageReceivedEventArgs e)
        {
            string[] cmd = e.Message.MessageText.Split(' ');
            if (cmd.Length > 1 && modules.ContainsKey(cmd[1].Trim()))
            {
                Module m = (Module)modules[cmd[1]];
                m.InterpretCommand(cmd, e);
            }

            if (e.Message.MessageText.StartsWith(" :$join"))
            {
                if (!adminList.Contains(e.Message.MessageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                    irc.Join(cmd[i]);
            }

            if (e.Message.MessageText.StartsWith(" :$leave"))
            {
                if (!adminList.Contains(e.Message.MessageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                    irc.Part(cmd[i]);
            }

            if (e.Message.MessageText.StartsWith(" :$quit"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.Message.MessageFrom + " attempted " + e.Message.MessageText.Substring(2));
                if (!adminList.Contains(e.Message.MessageFrom))
                    return;

                string reason = "";

                if (cmd.Length > 2)
                {
                    for (int i = 2; i < cmd.Length; i++)
                        reason += cmd[i].Replace("\r\n", " ") + " ";
                }
                else
                    reason = "No reason given";

                irc.Quit(reason);
            }

            if (e.Message.MessageText.StartsWith(" :$add_admin"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.Message.MessageFrom + " attempted " + e.Message.MessageText.Substring(2));
                if (e.Message.MessageFrom != mainAdmin)
                    return;

                for (int i = 2; i < cmd.Length; i++)
                {
                    adminList.Add(cmd[i].Trim());
                    Output.Write("ADMIN", ConsoleColor.Red, cmd[i].Trim() + " added to admins");

                }
            }

            if (e.Message.MessageText.StartsWith(" :$remove_admin"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.Message.MessageFrom + " attempted " + e.Message.MessageText.Substring(2));
                if (!adminList.Contains(e.Message.MessageFrom))
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

            if (e.Message.MessageText.StartsWith(" :$compliment_zebr"))
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, getCompliment()));

            if (e.Message.MessageText.StartsWith(" :$help") && e.Message.MessageText.Split(' ').Length <= 2)
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, @"Help can be found at http://graymalk.in/ircbot/"));

            if (e.Message.MessageText.StartsWith(" :$help") && e.Message.MessageText.Split(' ').Length > 2)
                GetHelp(e);

            if (e.Message.MessageText.StartsWith(" :$modules"))
                PrintModules(e);

            if (e.Message.MessageFrom == "zebr" && (e.Message.MessageText.Contains("Gwen: ") || e.Message.MessageText.Contains("Gwen ")))
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "zebr: <3"));


        }
        public IRC GetIRC() { return irc; }

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

        /// <summary>
        /// Work around for IRC servers awaiting an IDENT response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void irc_IdentNoAuthEvent(object sender, Events.IdentAuthNoResponseEventArgs e)
        {
            irc.Login();
        }

        /// <summary>
        /// Sends the modules enabled on the bot
        /// </summary>
        /// <param name="e"></param>
        private void PrintModules(Events.MessageReceivedEventArgs e)
        {
            string output = "";
            foreach(string s in modules.Keys)
            {
                output += s + " ";
            }
            output = output.Trim().Replace(":", "");
            irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, output));
        }

        /// <summary>
        /// Sends the help message for a given module.
        /// </summary>
        /// <param name="e"></param>
        private void GetHelp(Events.MessageReceivedEventArgs e)
        { 
            string m = ":" + e.Message.MessageText.Split(' ')[2];
            if(modules.ContainsKey(m))
            {
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, ((Module)modules[m]).Help()));
            }
        }
    }
}
