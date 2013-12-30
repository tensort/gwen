using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadedIRCBot
{
    public static class Output
    {
        public static void Write(string type, ConsoleColor color, string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = color;
            Console.Write(type);
            Console.ForegroundColor = ConsoleColor.White;
            string tmp = "";
            for (int i = 0; i < msg.Length; i++)
            {
                tmp += msg.ToCharArray()[i];
                if (i % 85 == 0 && i > 0)
                {
                    tmp += "\n\t\t\t   ";
                }
            }

            if (type.Length < 6)
                Console.Write(" {0}]\t\t   {1}\n", getTimeString(), tmp);
            else
                if (type.Length < 9)
                    Console.Write(" {0}]\t   {1}\n", getTimeString(), tmp);
                else
                    Console.Write(" {0}]\t   {1}\n", getTimeString(), tmp);
        }

        private static string getTimeString()
        {
            string time = "";

            if (DateTime.Now.Hour < 10)
                time += "0";
            time += DateTime.Now.Hour + ":";

            if (DateTime.Now.Minute < 10)
                time += "0";
            time += DateTime.Now.Minute;

            return time;
        }
    }
}
