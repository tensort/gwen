using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using System.Net;

namespace ThreadedIRCBot
{
    public class URL : Module
    {
        public static string URL_REGEX = @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*";

        public override void Start() { }

        public override void Stop() { }

        public override void Save() { }

        public URL(IRC ircNet) : base(ircNet)
        { }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            foreach(string url in getURLs(command))
            {
                string urlt = url.TrimStart(":".ToCharArray());
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Title: " + getTitle(urlt)));
            }
        }

        public override string Help()
        {
            return "This module listens for URLs in chat, and outputs titles or MIME types.";
        }

        public static List<String> getURLs(string[] words)
        {
            List<String> retVal = new List<string>();
            Regex re = new Regex(URL_REGEX);

            foreach(string s in words)
            {
                Match m = re.Match(s);
                if (m.Success)
                    retVal.Add(s);
            }

            return retVal;
        }

        public static string getTitle(string url)
        {
            HtmlDocument doc = new HtmlDocument();
            WebClient wc = new WebClient();
            doc.Load(wc.OpenRead(url));

            string retVal = doc.DocumentNode.SelectSingleNode("//head/title").InnerText;

            return retVal;
        }
    }
}
