using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ThreadedIRCBot
{
    class WorldTime : Module
    {
        public WorldTime(IRC ircNet) : base(ircNet) { }

        private static string apiKey = "CGo4VVfV34Gx5NISJtVqCTVSAMFYF0wMgMNvx.oTjeX5KQvfFHGEmN90vU.3Uwp.I3U-";

        public static string getTimeZone(string query)
        {
            string id = "";

            //XElement doc = XElement.Load("http://where.yahooapis.com/v1/places.q('" + query + "')?appid=" + apiKey);
            //doc = new 
            // id = doc.XPathSelectElement(@"woeid").Value;
            //XName woeidXName = "woeid";
            //id = doc.Element(woeidXName).Value;

            XNamespace xNamespace = "http://where.yahooapis.com/v1/schema.rng"; //replace this with namespace of prefix 'yweather'
            string url = "http://where.yahooapis.com/v1/places.q('" + query + "')?appid=" + apiKey;
            XDocument xDocument = XDocument.Load(url);
            var result = from item in xDocument.Descendants(xNamespace + @"timezone")
                         select item;
            foreach (XElement item in result)
            {
                id = item.Value;
            }

            return id;
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
            throw new NotImplementedException();
        }

        public override string Help()
        {
            return "Get the time in a given place.";
        }
    }
}
