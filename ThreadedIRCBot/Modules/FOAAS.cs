using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ThreadedIRCBot
{
    public class FOAAS : Module
    {
        public static string FOOAS_BASE_URL = @"http://foaas.herokuapp.com/";
        public FOAAS(IRC ircNet) : base(ircNet)
        {

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
            // Handle 1 var responses
            if (command.Length == 4 || command.Length == 5 && command[4] == "")
            {
                switch (command[2])
                {
                    case "you":
                    case "this":
                    case "that":
                    case "everything":
                    case "everyone":
                    case "pink":
                    case "life":
                    case "thing":
                    case "thanks":
                    case "flying":
                    case "fascinating":
                    case "cool":
                    case "what":
                    case "because":
                        string response = GetResponse(command[2] + "/" + command[3]);
                        irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, response));
                        return;
                    default: break;
                }
            }

            // Handle 2 var responses
            if (command.Length == 5 || command.Length == 6 && command[5] == "")
            {
                switch(command[2])
                {
                    case "off":
                    case "you":
                    case "donut":
                    case "shakespeare":
                    case "linus":
                    case "king":
                    case "nugget":
                    case "chainsaw":
                    case "outside":
                    case "yoda":
                    case "caniuse":
                        string response = GetResponse(command[2] + "/" + command[3] + "/" + command[4]);
                        irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, response));
                        return;
                    default: break;
                }
            }

            // Handle 3 var responses
            if (command.Length == 6 || command.Length == 7 && command[6] == "")
            {
                switch (command[2])
                {
                    case "field":
                    case "ballmer":
                        string response = GetResponse(command[2] + "/" + command[3] + "/" + command[4] + "/" + command[5]);
                        irc.Send(new IRCMessage("PRIVMSG", e.Message.MessageTarget, response));
                        return;
                    default: break;
                }
            }
        }

        public override string Help()
        {
            return "Calls methods from http://foaas.herokuapp.com/ where the command is $foaas <command> [options split by space]";
        }

        public static string GetResponse(string url)
        {
            WebRequest wr = HttpWebRequest.Create(FOOAS_BASE_URL + url);
            wr.Method = "GET";
            wr.Headers.Add("Accept", "text/plain");
            using (WebResponse resp = wr.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
//Test Name:	GetResponse
//Test FullName:	UnitTestProject1.FOOASTests.GetResponse
//Test Source:	c:\Users\Simon\Documents\Visual Studio 2013\Projects\gwen-master\UnitTestProject1\UnitTest1.cs : line 51
//Test Outcome:	Failed
//Test Duration:	0:00:00.0550547

//Result Message:	
//Test method UnitTestProject1.FOOASTests.GetResponse threw exception: 
//System.Net.ProtocolViolationException: Cannot send a content-body with this verb-type.
//Result StackTrace:	
//at System.Net.HttpWebRequest.CheckProtocol(Boolean onRequestStream)
//   at System.Net.HttpWebRequest.GetRequestStream(TransportContext& context)
//   at System.Net.HttpWebRequest.GetRequestStream()
//   at ThreadedIRCBot.FOAAS.GetResponse(String url) in c:\Users\Simon\Documents\Visual Studio 2013\Projects\gwen-master\ThreadedIRCBot\Modules\FOAAS.cs:line 108
//   at UnitTestProject1.FOOASTests.GetResponse() in c:\Users\Simon\Documents\Visual Studio 2013\Projects\gwen-master\UnitTestProject1\UnitTest1.cs:line 52

