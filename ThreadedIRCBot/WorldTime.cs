using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ThreadedIRCBot
{
    class WorldTime : Module
    {
        public WorldTime(IRC ircNet) : base(ircNet) { }

        private static string apiKey = "CGo4VVfV34Gx5NISJtVqCTVSAMFYF0wMgMNvx.oTjeX5KQvfFHGEmN90vU.3Uwp.I3U-";

        public struct LatLong
        {
            public string Latitude, Longitude;
            public LatLong(string lat, string lng)
            {
                Latitude = lat;
                Longitude = lng;
            }
        }

        public static LatLong getTimeZone(string query)
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
            var result = from item in xDocument.Descendants(xNamespace + @"latitude")
                         select item;
            foreach (XElement item in result)
            {
                id = item.Value;
            }
            string lat = id;

            result = from item in xDocument.Descendants(xNamespace + @"longitude")
                         select item;
            foreach (XElement item in result)
            {
                id = item.Value;
            }
            string lng = id;

            return new LatLong(lat, lng);
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
            if(command.Length > 2)
            {
                string place = "";
                for (int i = 2; i < command.Length; i++)
                    place += command[i];

                // Time in given place.
                Output.Write("World Time", ConsoleColor.Yellow, "Attempting to get world time for " + command[2]);
                LatLong location = getTimeZone(place);

                // Use the GeoNames API to get the current time
                // http://api.geonames.org/timezone?lat=51&lng=0&username=graymalkin
                XDocument xDocument = XDocument.Load("http://api.geonames.org/timezone?lat=" + location.Latitude + 
                        "&lng=" + location.Longitude + "&username=graymalkin");

                string time = "";
                var result = from item in xDocument.Descendants(@"time")
                             select item;
                foreach (XElement item in result)
                {
                    time = item.Value;
                }

                try
                {
                    DateTime t = DateTime.Parse(time);
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Time in " + place + ": " + t.ToString("HH:mm")));
                }
                catch
                {
                    irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, "Could not get time for " + place));
                }
            }
            else
            {
                if(command.Length == 2)
                {
                    // We're looking at ":$time" on it's own - give local time.
                }
            }
        }

        public override string Help()
        {
            return "Get the time in a given place.";
        }
    }
}
