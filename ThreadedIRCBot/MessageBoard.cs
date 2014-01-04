using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ThreadedIRCBot
{   
    class MessageBoard : Module
    {
        public MessageBoard(IRC ircNet) : base(ircNet) { }

        string Message = "";
        public override void Start()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("http://tinkering.graymalk.in/messageBoard/board.txt");
            StreamReader reader = new StreamReader(stream);
            Message = reader.ReadToEnd();
            Output.Write("Message Board", ConsoleColor.Green, Message);
        }

        public override void Stop()
        {
        }

        public override void Save()
        {
        }

        public override void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e)
        {
            string s = "";

            if (command.Length == 2)
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://tinkering.graymalk.in/messageBoard/board.txt");
                StreamReader reader = new StreamReader(stream);
                Message = reader.ReadToEnd();


                irc.send(new Message("PRIVMSG", e.message.messageTarget, "Message Board Set to: \"" + Message + "\""));
                return;
            }

            for (int i = 2; i < command.Length; i++)
                s += command[i] + " ";

            Message = s.Trim();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://tinkering.graymalk.in/messageBoard/?apiKey=K1pnBbbBuXjACwt5BOQ2bxOcoyfiNEvh9DWcChQnMD5gjz9Lb4NYZ9yUbkpvVqv&message=" + Message);
            request.GetResponse();

            irc.send(new Message("PRIVMSG", e.message.messageTarget, "Message Board Set to: \"" + Message + "\""));
        }

        public override string Help()
        {
            return "Get or set the shed's message board text";
        }
    }
}
