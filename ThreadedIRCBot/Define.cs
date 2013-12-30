using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ThreadedIRCBot
{
    class Define : Module
    {
        public Define(IRC ircNet) : base(ircNet) { }

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
            if(command.Length > 2)
            {
                try
                {
                    irc.send(new Message("PRIVMSG", e.message.messageTarget, DefineWord(command[2])));
                }
                catch (Exception exc)
                {
                    Output.Write("Define", ConsoleColor.Red, exc.Message + " " + exc.StackTrace);
                    irc.send(new Message("PRIVMSG", e.message.messageTarget, "Could not define: " + command[2]));
                }
            }
        }

        public static string DefineWord(string word)
        {
            Output.Write("Define", ConsoleColor.Yellow, word);
            SkPublishAPI api = new SkPublishAPI("http://dictionary.cambridge.org/api/v1/", "nFDGeMlZeWoM54FxNT4X7hOloAZ6DrddUf6XPzHvTzxu4XMdKwyJPOGWHR0EftmE");

            IList<IDictionary<string, object>> dictionaries = JsonToArray(api.GetDictionaries());
            IDictionary<string, object> dictionary = dictionaries[0];

            string dictionaryCode = (string)dictionary["dictionaryCode"];
            Output.Write("Define", ConsoleColor.Yellow, dictionaryCode);

            IDictionary<string, object> bestMatch = JsonToObject(api.SearchFirst(dictionaryCode, word, "xml"));
            word = (string) bestMatch["entryId"];

            IDictionary<string, object> definition = JsonToObject(api.GetEntry(dictionaryCode, word, "xml"));
            return XMLEntryToString((string)definition["entryContent"]);
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
                output += "Definition of " + xmlReader.ReadElementContentAsString() + ": ";
                xmlReader.ReadToFollowing("def");
                output += xmlReader.ReadInnerXml();
                output = output.Replace("<Ref>", "");
                output = output.Replace("</Ref>", "");
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
