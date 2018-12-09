using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DSInjector
{
    internal class Server
    {
        public bool didntClosing;
        private ManualResetEvent allDone = new ManualResetEvent(false);
        public bool work = false;
        private Socket sListener;
        public static List<string> files_hash;

        public static Server.Callback OnReceived { get; set; }

        public Server()
        {
            new Thread((ThreadStart) (() => this.StartListening())).Start();
        }

        private void StartListening()
        {
            didntClosing = true;
            Server.files_hash = new List<string>();
            this.work = true;
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(address, 11000);
            this.sListener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.sListener.Bind((EndPoint) ipEndPoint);
            this.sListener.Listen(10);
            while (didntClosing)
            {
                this.allDone.Reset();
                this.sListener.BeginAccept(new AsyncCallback(this.AcceptCallback), (object)this.sListener);
                this.allDone.WaitOne();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            this.allDone.Set();
            if (!this.work)
                return;
            Socket socket = ((Socket) ar.AsyncState).EndAccept(ar);
            Server.StateObject stateObject = new Server.StateObject();
            stateObject.workSocket = socket;
            socket.BeginReceive(stateObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.ReadCallback), (object) stateObject);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            Server.StateObject asyncState = (Server.StateObject) ar.AsyncState;
            Socket workSocket = asyncState.workSocket;
            int count = workSocket.EndReceive(ar);
            if (count <= 0)
                return;
            asyncState.sb.Append(Encoding.ASCII.GetString(asyncState.buffer, 0, count));
            string input = asyncState.sb.ToString();
            if (input.IndexOf("<ENDRECIVE_SERVER>") > -1)
            {
                string str = Regex.Replace(input, "(<ENDRECIVE_SERVER>)", string.Empty);
                if (str != "")
                {
                    string[] strArray = str.Split("|".ToCharArray());
                    string s = "0";
                    if (!Server.files_hash.Contains(strArray[0]))
                    {
                        if (Server.OnReceived != null)
                            Server.OnReceived(strArray[1]);
                            Server.files_hash.Add(strArray[0]);
                            s = "1";
                    }
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    workSocket.Send(bytes);
                }
            }
            else
                workSocket.BeginReceive(asyncState.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.ReadCallback), (object) asyncState);
        }

        public void Stop()
        {
            this.work = false;
            this.sListener.Close();
        }

        public delegate void Callback(string data);

        public class StateObject
        {
            public Socket workSocket = (Socket) null;
            public byte[] buffer = new byte[1024];
            public StringBuilder sb = new StringBuilder();
            public const int BufferSize = 1024;
        }
    }
}
