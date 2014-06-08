using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;

namespace ThreadedIRCBot
{
    class Weather : Module
    {
        public Weather(IRC ircNet) : base(ircNet)
        {
            
        }

        public override void Start() { }

        public override void Stop() { }

        public override void Save() { }

        public override string Help()
        {
            return "Get the weather for a given place";
        }

        public override void InterpretCommand(string[] cmd, Events.MessageReceivedEventArgs e)
        {
            IRCMessage response;
            string msg = "", query = "";

            if (cmd.Length > 2)
            {
                for (int i = 2; i < cmd.Length; i++)
                    query += cmd[i].Replace("\r\n", " ") + " ";
            }
            msg = Weather.getWeather(query);
            response = new IRCMessage("PRIVMSG", e.Message.MessageTarget, msg);

            irc.Send(response);
        }



        private static string apiKey = "CGo4VVfV34Gx5NISJtVqCTVSAMFYF0wMgMNvx.oTjeX5KQvfFHGEmN90vU.3Uwp.I3U-";
        public static string getWeather()
        {
            return getWeather("Canterbury");
        }

<<<<<<< HEAD
        public static string getWeather(string locaiton)
        {
            if (locaiton == "")
                locaiton = "Canterbury";

            string resultString = "Weather for ";
            char degree = (char)176;    

            Output.Write("PROCESSING", ConsoleColor.Yellow, resultString + " " + locaiton);
            locaiton = locaiton.Trim();
=======
        public static string getWeather(string location)
        {
            if (location == "")
                location = "Canterbury";

            string resultString = "Weather for ";
            char degree = (char)176;

            Output.Write("PROCESSING", ConsoleColor.Yellow, resultString + " " + location);
            location = location.Trim();
>>>>>>> c2b1780db45bc776676895cc5fa13d53f935d5f1

            try  
            {
                XNamespace xNamespace = "http://xml.weather.yahoo.com/ns/rss/1.0"; //replace this with namespace of prefix 'yweather'
<<<<<<< HEAD
                XDocument xDocument = XDocument.Load(@"http://weather.yahooapis.com/forecastrss?u=c&w=" + getWOEID(HttpUtility.UrlEncode(locaiton)));
=======
                XDocument xDocument = XDocument.Load(@"http://weather.yahooapis.com/forecastrss?u=c&w=" + getWOEID(HttpUtility.UrlEncode(location)));
>>>>>>> c2b1780db45bc776676895cc5fa13d53f935d5f1

                var result = from item in xDocument.Descendants(xNamespace + "location")
                             select item;

                foreach (XElement item in result)
                {
                    resultString += item.Attribute("city").Value + ", ";
                    if (item.Attribute("region").Value != "")
                        resultString += item.Attribute("region").Value + ", ";
                    resultString += item.Attribute("country").Value +  ": ";
                }


                result = from item in xDocument.Descendants(xNamespace + "condition")
                             select item;

                foreach (XElement item in result)
                {
                    resultString += item.Attribute("text").Value + ", ";
                    resultString += item.Attribute("temp").Value + degree + "C. ";
                }

                result = from item in xDocument.Descendants(xNamespace + "wind")
                         select item;

                foreach (XElement item in result)
                {
                    resultString += "Wind Speed " + item.Attribute("speed").Value + "km/h, with a ";
                    resultString += item.Attribute("chill").Value + degree + "C Wind Temp. ";
                }

                result = from item in xDocument.Descendants(xNamespace + "atmosphere")
                         select item;

                foreach (XElement item in result)
                {
                    resultString += "Humidity " + item.Attribute("humidity").Value + "%, Visibility ";
                    resultString += item.Attribute("visibility").Value + "km.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }

            if (resultString != "Weather for ")
                return resultString;
            else
<<<<<<< HEAD
                return "Could not find " + locaiton;
=======
                return "Could not find " + location;
>>>>>>> c2b1780db45bc776676895cc5fa13d53f935d5f1

        }

        public static string getWOEID(string query)
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
            var result = from item in xDocument.Descendants(xNamespace + @"woeid")
                         select item;
            foreach (XElement item in result)
            {
                id = item.Value;
            }

            return id;            
        }
    }
}
