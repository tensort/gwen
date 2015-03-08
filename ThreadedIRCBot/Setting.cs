using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    public class Setting
    {
        public string IRCName { get; set; }
        public string IRCHost { get; set; }
        public string IRCNick { get; set; }
        public string IRCUser { get; set; }
        public int IRCPort { get; set; }
        public List<string> AutoJoin { get; set; }
        public List<string> Admins { get; set; }
    }
}
