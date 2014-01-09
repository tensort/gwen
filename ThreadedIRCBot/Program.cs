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
            b.AddModule(new Weather(b.GetIRC()), "$weather");
            b.AddModule(new Define(b.GetIRC()), "$d");
            b.AddModule(new MessageBoard(b.GetIRC()), "$mboard");
            b.AddModule(new Roll(b.GetIRC()), "$roll");
            b.AddModule(new Ning(b.GetIRC()), "$ning");
            b.GetIRC().Connect();
        }
    }

}