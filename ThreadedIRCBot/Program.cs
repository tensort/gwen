using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot b = new Bot(args);
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

            b.AddListener(new URL(b.GetIRC()));

            b.GetIRC().Connect();
        }
    }

}