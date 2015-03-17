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
	List<string> ignoreList;

        Hashtable modules = new Hashtable();
        List<Module> listeners = new List<Module>();

        public Bot(string ircNet, string ircName, string ircNick, string realName, int port, List<string> admins, List<string> ignores)
        {
            irc = new IRC(ircNet, ircName, ircNick, realName, port);
            irc.IdentNoAuthEvent += new IRC.IdentNoAuthEventHandler(irc_IdentNoAuthEvent);  // Work around
            irc.MessageEvent += new IRC.MessageEventHandler(irc_MessageEvent);              // Kinda essential.

            adminList = new List<string>(admins);
	    ignoreList = new List<string>(ignores);
        }

        public IRC GetIRC() { return irc; }

        public void AddModule(Module m, string command)
        {
            if (modules[":" + command] != null)
                modules[":" + command] = m;
            else
                modules.Add(":" + command, m);
        }

        public void AddListener(Module m)
        {
            listeners.Add(m);
        }

        public bool IsModuleNameReserved(string command)
        {
            bool inSetAlready = (modules[":" + command] != null);
            if(inSetAlready)
                return !(modules[":" + command] is EventTimer);
            return false;
        }

        void irc_MessageEvent(object sender, Events.MessageReceivedEventArgs e)
        {
            string[] cmd = e.Message.MessageText.Split(' ');

            foreach(Module m in listeners)
                m.InterpretCommand(cmd, e);

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
                if (!IsAdmin(e.Message.MessageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                {
                    adminList.Add(cmd[i].Trim());
                    Output.Write("ADMIN", ConsoleColor.Red, cmd[i].Trim() + " added to admins");
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Ok."));
                }
            }

            if (e.Message.MessageText.StartsWith(" :$remove_admin"))
            {
                Output.Write("ADMIN", ConsoleColor.Red, e.Message.MessageFrom + " attempted " + e.Message.MessageText.Substring(2));
                if (!IsAdmin(e.Message.MessageFrom))
                    return;

                for (int i = 2; i < cmd.Length; i++)
                {
                    adminList.Remove(cmd[i].Trim());
                    Output.Write("ADMIN", ConsoleColor.Red, cmd[i].Trim() + " removed from admins");
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Ok."));
                }
            }

            if (e.Message.MessageText.StartsWith(" :$help") && e.Message.MessageText.Split(' ').Length <= 2)
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, @"Help can be found at http://graymalk.in/ircbot/"));

            if (e.Message.MessageText.StartsWith(" :$help") && e.Message.MessageText.Split(' ').Length > 2)
                GetHelp(e);

            if (e.Message.MessageText.StartsWith(" :$modules"))
                PrintModules(e);
        }

        /// <summary>
        /// Work around for IRC servers awaiting an IDENT response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void irc_IdentNoAuthEvent(object sender, Events.IdentAuthNoResponseEventArgs e)
        {
            irc.Login(new List<String>());
	    foreach(string ignore in ignoreList)
	    	    irc.Send(new IRCMessage("IGNORE", ignore));
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

        public bool IsAdmin(string Nick)
        {
            return adminList.Contains(Nick);
        }
    }
}
