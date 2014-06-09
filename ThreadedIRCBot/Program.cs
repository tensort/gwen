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
            Define d = new Define(b.GetIRC());
            b.AddModule(d, "$d");
            b.AddModule(d, "$p");
            b.AddModule(d, "$wotd");

            b.GetIRC().Connect();
        }
    }

}