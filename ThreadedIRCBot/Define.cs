using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace ThreadedIRCBot
{
    class Define : Module
    {
        public Define(IRC ircNet) : base(ircNet) { }
        private const string apiURL = @"https://dictionary.cambridge.org/api/v1/";
        private const string apiKey = @"X2kG7tRdNrGGe4dZS2CcSTo9aVjGbr4kCYRgm5C8cOq8hrKeY5ATkhd2vz2WX8Wd";

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

        public override string Help()
        {
            return "Define a given word.";
        }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            if(command.Length == 2)
            {
                try
                {
                    switch (command[1])
                    {
                        case ":$wotd":
                            irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, GetWordOfTheDay()));
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Output.Write("Define", ConsoleColor.Red, exc.Message + " " + exc.StackTrace);
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Could not get word of the day."));
                }
            }
            if(command.Length > 2)
            {
                try
                {
                    switch (command[1])
                    {
                        case ":$d":
                            irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, DefineWord(command[2])));
                            break;
                        case ":$p":
                            irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, GetPronunciation(command[2])));
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Output.Write("Define", ConsoleColor.Red, exc.Message + " " + exc.StackTrace);
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Could not define: " + command[2]));
                }
            }
        }

        public static string DefineWord(string word)
        {
            Output.Write("Define\t", ConsoleColor.Yellow, word);

            SkPublishAPI api = new SkPublishAPI(apiURL, apiKey);

            IList<IDictionary<string, object>> dictionaries = JsonToArray(api.GetDictionaries());
            IDictionary<string, object> dictionary = dictionaries[0];

            string dictionaryCode = (string)dictionary["dictionaryCode"];
            Output.Write("Define", ConsoleColor.Yellow, dictionaryCode);

            IDictionary<string, object> bestMatch = JsonToObject(api.SearchFirst(dictionaryCode, word, "xml"));
            string bestWord = (string) bestMatch["entryId"];

            IDictionary<string, object> definition = JsonToObject(api.GetEntry(dictionaryCode, bestWord, "xml"));
            return "Definition of " + word + ": " + XMLEntryToString((string)definition["entryContent"]);
        }

        public static string GetPronunciation(string word)
        {
            Output.Write("Pronunciation", ConsoleColor.Yellow, word);
            SkPublishAPI api = new SkPublishAPI(apiURL, apiKey);

            IList<IDictionary<string, object>> dictionaries = JsonToArray(api.GetDictionaries());
            IDictionary<string, object> dictionary = dictionaries[0];

            string dictionaryCode = (string)dictionary["dictionaryCode"];

            IDictionary<string, object> bestMatch = JsonToObject(api.SearchFirst(dictionaryCode, word, "xml"));
            word = (string)bestMatch["entryId"];

       
            var data = new NameValueCollection();
            data["accessKey"] = apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL + "dictionaries/" + dictionaryCode + "/entries/" + word + "/pronunciations");
            request.Headers.Add(data);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader r = new StreamReader(response.GetResponseStream());
            String s = r.ReadToEnd();

            Console.WriteLine(s);

            IList<IDictionary<string, object>> ans = JsonToArray(s);
            s = (string)ans[0]["pronunciationUrl"];
            return "Pronounciation of " + word + ": " + s;
        }

        public static string GetWordOfTheDay()
        {
            Output.Write("Word of Day", ConsoleColor.Yellow, "");
            
            SkPublishAPI api = new SkPublishAPI(apiURL, apiKey);
            /*
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            IList<IDictionary<string, object>> dictionaries = JsonToArray(api.GetDictionaries());
            IDictionary<string, object> dictionary = dictionaries[0];

            string dictionaryCode = (string)dictionary["dictionaryCode"];

            IList<IDictionary<string, object>> bestMatch = JsonToArray(api.GetWordOfTheDay("", today, "xml"));
            word = (string)bestMatch[0]["entryId"];

            IDictionary<string, object> definition = JsonToObject(api.GetEntry(dictionaryCode, word, "xml"));
            return (string)bestMatch[0]["entryLabel"] + ": " + (string)definition["entryContent"];
            /*/

            var data = new NameValueCollection();
            data["accessKey"] = apiKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL + "wordoftheday");
            request.Headers.Add(data);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader r = new StreamReader(response.GetResponseStream());
            String s = r.ReadToEnd();

            IDictionary<string, object> wotd = JsonToObject(s);
            s = (string)wotd["entryLabel"];

            IDictionary<string, object> definition = JsonToObject(api.GetEntry((string)wotd["dictionaryCode"], (string)wotd["entryId"], "xml"));
            string def = XMLEntryToString((string)definition["entryContent"]);

            return "Word of the day: " + s + " - " + def;
            // */
        }

        private static IDictionary<string, object> JsonToObject(string json)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }

        private static IList<IDictionary<string, object>> JsonToArray(string json)
        {
            return JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(json);
        }

        private static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private static string XMLEntryToString(string xml)
        {
            string output = "";
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml)))
            {
                 xmlReader.ReadToFollowing("title");
                output += xmlReader.ReadElementContentAsString() + ": ";
                xmlReader.ReadToFollowing("def");
                output += xmlReader.ReadInnerXml();
                output = output.Replace("<Ref>", "");
                output = output.Replace("</Ref>", "");
                output = output.Replace("<x>", "");
                output = output.Replace("</x>", "");
            }

            return output;
        }

        private static string XMLGetBestMatch(string xml)
        {
            string output = "";
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xml)))
            {
                xmlReader.ReadToFollowing("title");
                output = xmlReader.ReadElementContentAsString();
            }

            return output;
        }
    }
}
