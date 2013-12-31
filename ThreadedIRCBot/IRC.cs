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
        public void connect()
        {
            Output.Write("CONNECTION", ConsoleColor.Red, "Attempting to connect...");

            tcpClient = new TcpClient();
            AsyncCallback connectCallback = new AsyncCallback(ASyncConnectCallback);
            connecting = true;
            tcpClient.BeginConnect(chatnet, port, ASyncConnectCallback, connecting);

            while (connecting)
                Thread.Sleep(200);

            while (tcpClient.Connected)
            {
                if (tcpClient.Available > 0)
                {
                    AsyncCallback readCallback = new AsyncCallback(ASyncReadCallback);
                    byte[] buffer = new byte[tcpClient.Available];
                    networkStream.BeginRead(buffer, 0, tcpClient.Available, readCallback, buffer);
                }
                Thread.Sleep(200);
            }

            Output.Write("CONNECTION", ConsoleColor.Red, "Disconnected.");
            Console.ReadLine();
        }

        /// <summary>
        /// Logs into the IRC network
        /// </summary>
        public void login()
        {
            send("NICK " + nickname);
            send("USER " + nickname + " " + netname + " " + netname + " :" + realname);
        }

        /// <summary>
        /// Joins a channel on the IRC Network
        /// </summary>
        /// <param name="channelName">A given channel name</param>
        public void join(string channelName)
        {
            send("JOIN " + channelName);
        }

        /// <summary>
        /// Leaves a channel on the IRC Network
        /// </summary>
        /// <param name="channelName">A given channel name</param>
        public void part(string channelName)
        {
            send("PART " + channelName);
        }

        /// <summary>
        /// Quits the IRC network, and close the TCP IP connections
        /// </summary>
        /// <param name="reason">The reason for quitting</param>
        public void quit(string reason)
        {
            send("QUIT :" + reason);
            networkStream.Close();
            tcpClient.Close();
        }

        /// <summary>
        /// Send a Message over IRC
        /// </summary>
        /// <param name="m">Message to send</param>
        public void send(Message m)
        {
            send(Encoding.UTF8.GetString(m.toByteArray()));
        }

        /// <summary>
        /// Send a raw string encoded in UTF8
        /// </summary>
        /// <param name="msg">String to encode and send</param>
        private void send(string msg)
        {
            networkStream.Flush();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg + "\r\n");

            networkStream.BeginWrite(data, 0, data.Length, ASyncSendCallback, msg);
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
            login();
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
                createMessageEvent(sanitisedLn);
            }
        }
        #endregion

        private void createMessageEvent(string text)
        {            
            if (text.Contains("NOTICE AUTH :*** No Ident response"))
            {
                // Create new ident request no response event
                IdentNoAuthEvent(this, new Events.IdentAuthNoResponseEventArgs());
                return;
            }

            if (text.StartsWith("PING"))
                send(text.Replace("PING :", "PONG "));
            else
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
                MessageEvent(this, new Events.MessageReceivedEventArgs(new Message(command, target, msg, from)));
            }
        }
    }
}
