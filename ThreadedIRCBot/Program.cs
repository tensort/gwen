using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using Newtonsoft.Json;

namespace ThreadedIRCBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Output.Write("Error", ConsoleColor.Red, "Please provide a settings.json file");
                return;
            }

            JsonSerializer serializer = new JsonSerializer();
            List<Setting> netSetting;
            using (FileStream fs = new FileStream(args[0], FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                netSetting = (List<Setting>)serializer.Deserialize(sr, typeof(List<Setting>));
            }

            foreach (Setting s in netSetting)
            {
                Bot b = new Bot(s.IRCHost, s.IRCName, s.IRCNick, s.IRCUser, s.IRCPort, s.Admins);
                b.AddModule(new WorldTime(b.GetIRC()), "$time");
                b.AddModule(new Weather(b.GetIRC()), "$weather");
                b.AddModule(new MessageBoard(b.GetIRC()), "$mboard");
                b.AddModule(new Roll(b.GetIRC()), "$roll");
                b.AddModule(new Ning(b.GetIRC()), "$ning");
                b.AddModule(new FOAAS(b.GetIRC()), "$foaas");
                b.AddModule(new Timer(b.GetIRC(), b), "$timer");
                Define d = new Define(b.GetIRC());
                b.AddModule(d, "$d");
                b.AddModule(d, "$p");
                b.AddModule(d, "$wotd");

                b.AddListener(new URL(b.GetIRC(), !s.IRCHost.Contains("freenode")));

                b.GetIRC().Connect(s.AutoJoin);
            }

            // TODO: Horrible hack.
            while (true)
                Thread.Sleep(200);
        }
    }

}