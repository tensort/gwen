using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ThreadedIRCBot
{
    class IRC
    {
        public delegate void MessageEventHandler(object sender, Events.MessageReceivedEventArgs e);
        public event MessageEventHandler MessageEvent;
        public delegate void IdentNoAuthEventHandler(object sender, Events.IdentAuthNoResponseEventArgs e);
        public event IdentNoAuthEventHandler IdentNoAuthEvent;

        private string chatnet, nickname, realname, netname;
        private int port;
        bool connecting;

        TcpClient tcpClient;
        NetworkStream networkStream;
        
        /// <summary>
        /// Sets up a new IRC server
        /// </summary>
        /// <param name="_chatNet">The URL of the IRC server to connect to</param>
        /// <param name="_netname">The name of the IRC Network</param>
        /// <param name="_nickname">The desired name on this network</param>
        /// <param name="_realname">A real name</param>
        /// <param name="_port">The port of the IRC server</param>
        public IRC(string _chatNet, string _netname, string _nickname, string _realname, int _port)
        {
            chatnet = _chatNet;
            port = _port;
            realname = _realname;
            nickname = _nickname;
            netname = _netname;
        }

        /// <summary>
        /// Opens the connection to the IRC Network
        /// </summary>
        public void Connect()
        {
            Output.Write("CONNECTION", ConsoleColor.Red, "Attempting to connect...");

            tcpClient = new TcpClient();
            AsyncCallback connectCallback = new AsyncCallback(ASyncConnectCallback);
            connecting = true;
            tcpClient.BeginConnect(chatnet, port, ASyncConnectCallback, connecting);

            while (connecting)
                Thread.Sleep(200);

            while (tcpClient.Connected)         // While we still have a connection
            {
                if (tcpClient.Available > 0)    // Check to see if there's any data to read
                {
                    // Create a new callback for the read data
                    AsyncCallback readCallback = new AsyncCallback(ASyncReadCallback);

                    // Create a buffer for the data

                    byte[] buffer = new byte[tcpClient.Available];

                    // Read the data into the buffer, and pass it to the callback method to deal with asychronously 
                    networkStream.BeginRead(buffer, 0, tcpClient.Available, readCallback, buffer);
                }
                #if __MonoCS__
                    // Sleeps the thread for 50ms (blocking read does not work under mono)
                    Thread.Sleep(50);
                #else   
                    // Read 0 bytes from the buffer, and allow this to block
                    networkStream.Read(new byte[1], 0, 0);
                #endif
            }

            Output.Write("CONNECTION", ConsoleColor.Red, "Disconnected.");
            Console.ReadLine();
        }

        /// <summary>
        /// Logs into the IRC network
        /// </summary>
        public void Login()
        {
            Send("NICK " + nickname);
            Send("USER " + nickname + " " + netname + " " + netname + " :" + realname);
        }

        /// <summary>
        /// Joins a channel on the IRC Network
        /// </summary>
        /// <param name="channelName">A given channel name</param>
        public void Join(string channelName)
        {
            Send("JOIN " + channelName);
        }

        /// <summary>
        /// Leaves a channel on the IRC Network
        /// </summary>
        /// <param name="channelName">A given channel name</param>
        public void Part(string channelName)
        {
            Send("PART " + channelName);
        }

        /// <summary>
        /// Quits the IRC network, and close the TCP IP connections
        /// </summary>
        /// <param name="reason">The reason for quitting</param>
        public void Quit(string reason)
        {
            Send("QUIT :" + reason);
            networkStream.Close();
            tcpClient.Close();
        }

        /// <summary>
        /// Send a Message over IRC
        /// </summary>
        /// <param name="m">Message to send</param>
        public void Send(IRCMessage m)
        {
            Send(Encoding.UTF8.GetString(m.ToByteArray()));
        }

        /// <summary>
        /// Send a raw string encoded in UTF8
        /// </summary>
        /// <param name="msg">String to encode and send</param>
        private void Send(string msg)
        {
            try
            {
                networkStream.Flush();
                byte[] data = System.Text.Encoding.UTF8.GetBytes(msg + "\r\n");

                networkStream.BeginWrite(data, 0, data.Length, ASyncSendCallback, msg);
            }
            catch (NullReferenceException)
            {
                Output.Write("ERROR", ConsoleColor.Red, "Network stream doesn't exist.");
            }
        }

        #region ASyncCallbacks
        private void ASyncSendCallback(IAsyncResult result)
        {
            string m = (string)result.AsyncState;
            Output.Write("SENT", ConsoleColor.Red, m);
        }

        private void ASyncConnectCallback(IAsyncResult result)
        {
            if (tcpClient.Connected)
                networkStream = tcpClient.GetStream();
            else
                Output.Write("CONNECTION", ConsoleColor.Red, "Failed to connect to remote server");

            tcpClient.LingerState = new LingerOption(false, 0);
            Login();
            connecting = false;
        }

        private void ASyncReadCallback(IAsyncResult result)
        {
            byte[] data = (byte[])result.AsyncState;
            string utf8Encoded = System.Text.Encoding.UTF8.GetString(data);
            // Split the string into it's lines
            string[] lines = utf8Encoded.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string ln in lines)
            {
                string sanitisedLn = ln.Replace("\r\n", "");
                Output.Write("RECEIVED", ConsoleColor.Green, sanitisedLn);
                CreateMessageEvent(sanitisedLn);
            }
        }
        #endregion

        private void CreateMessageEvent(string text)
        {            

            if (text.Contains("NOTICE AUTH :*** No Ident response"))
            {
                // Create new ident request no response event
                IdentNoAuthEvent(this, new Events.IdentAuthNoResponseEventArgs());
                return;
            }

            if (text.StartsWith("PING"))
                Send(text.Replace("PING :", "PONG "));
            else
            {
                if (text.Split(' ').Length > 3)
                {  
                    string msg = "", target = "", command = "", from = "";
                    command = text.Split(' ')[1];
                    if (command == "PRIVMSG")
                        from = text.Split(' ')[0].Split(':')[1].Split('!')[0];
                    target = text.Split(' ')[2];
                    for (int i = 3; i < text.Split(' ').Length; i++)
                    {
                        msg = msg + " " + text.Split(' ')[i];
                    }
                    MessageEvent(this, new Events.MessageReceivedEventArgs(new IRCMessage(command, target, msg, from)));
                }
            }
        }
    }
}
