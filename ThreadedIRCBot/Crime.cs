using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadedIRCBot.Events;

namespace ThreadedIRCBot
{
    /// <summary>
    /// 
    /// </summary>
    class Crime : Module
    {
        public Crime(IRC ircNet) : base(ircNet)
        {
            
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override string Help()
        {
            return "UK Crime Information";
        }

        public override void InterpretCommand(string[] command, MessageReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
