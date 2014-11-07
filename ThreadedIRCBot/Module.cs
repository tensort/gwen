using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    public abstract class Module
    {
        protected IRC irc;

        /// <summary>
        /// Constructor, with a reference passed for the IRC object this module will talk to
        /// </summary>
        /// <param name="ircNet">The IRC net object this module will talk to</param>
        public Module(IRC ircNet)
        {
            irc = ircNet;
        }

        /// <summary>
        /// Start the module
        /// </summary>
        abstract public void Start();

        /// <summary>
        /// Stop the module
        /// </summary>
        abstract public void Stop();

        /// <summary>
        /// Save any settings which may need to be saved
        /// </summary>
        abstract public void Save();

        /// <summary>
        /// The handler for interupreting commands
        /// </summary>
        /// <param name="command">Command arguements</param>
        abstract public void InterpretCommand(string[] command, Events.MessageReceivedEventArgs e);

        abstract public string Help();
    }
}
