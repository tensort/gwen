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
<<<<<<< HEAD
            b.AddModule(new MessageBoard(b.GetIRC()), "$mboard");
            b.AddModule(new Roll(b.GetIRC()), "$roll");
            b.AddModule(new Ning(b.GetIRC()), "$ning");
            Define d = new Define(b.GetIRC());
            b.AddModule(d, "$d");
            b.AddModule(d, "$p");
            b.AddModule(d, "$wotd");
=======
            b.AddModule(new Define(b.GetIRC()), "$d");
            b.AddModule(new MessageBoard(b.GetIRC()), "$mboard");
            b.AddModule(new Roll(b.GetIRC()), "$roll");
            b.AddModule(new Ning(b.GetIRC()), "$ning");
>>>>>>> c2b1780db45bc776676895cc5fa13d53f935d5f1
            b.GetIRC().Connect();
        }
    }

}