using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

using HtmlAgilityPack;
using iTextSharp.text.pdf;
using System.Net;

namespace ThreadedIRCBot
{
    public class URL : Module
    {
        public static string URL_REGEX = @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*";
        public static Int64 MAX_DOWNLOAD_SIZE = 1024 * 1024 * 64; // Max download 64 mb
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
                irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, getTitle(urlt)));
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
            // We should keep track of how many URLs have been added, so we can limit for flood prevention.
            int match_count = 0;
            
            foreach(string s in words)
            {
                Match m = re.Match(s);
                if (m.Success && ++match_count <= 5)
                    retVal.Add(s);
            }

            return retVal;
        }

        public static string getTitle(string url)
        {
            Int64 bytes_total;

            WebRequest wr = HttpWebRequest.Create(url);
            // Set the 'Timeout' property in Milliseconds.
            wr.Timeout = 5000; // 5s timeout
            wr.Method = "HEAD";
            using (WebResponse wresp = wr.GetResponse())
            {
                bytes_total = Convert.ToInt64(wresp.Headers["Content-Length"]);
                string size_string = size_to_string(bytes_total);
                if (bytes_total > MAX_DOWNLOAD_SIZE)
                {
                    return "[" + wresp.Headers["Content-Type"] + " " + size_to_string(bytes_total) + "]";
                }
          

		if(wresp.Headers["Content-Type"] == "application/pdf")
	    	{
		    PdfReader reader = new PdfReader(url);
		    try
		    {
			var title = reader.Info["Title"];
		        return "Title: " + title + " [" + wresp.Headers["Content-Type"] + " " + size_to_string(bytes_total) + "]";
		    }
		    catch
		    {
			return "[" + wresp.Headers["Content-Type"] + " " + size_to_string(bytes_total) + "]";
		    }
		}
	    }

            HtmlDocument doc = new HtmlDocument();
            WebClient wc = new WebClient();
            // Lie about the user agent, to stop websites trying to be clever.
            wc.Headers["User-Agent"] = "Mozilla/5.0 (MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            doc.Load(wc.OpenRead(url), Encoding.UTF8);
            string retVal = "";


            if (doc.DocumentNode.SelectSingleNode("//head/title") != null)
            {
                retVal = "Title: " + doc.DocumentNode.SelectSingleNode("//head/title").InnerText.Trim();
            }
            else
            {
                return "[" + wc.ResponseHeaders["content-type"] + " " + size_to_string(bytes_total) + "]";
            }
            return HtmlEntity.DeEntitize(retVal);
        }

        public static string size_to_string(Int64 byteCount)
        {
            string[] suf = { " B", " KiB", " MiB", " GiB", " TiB", " PiB", " EiB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
