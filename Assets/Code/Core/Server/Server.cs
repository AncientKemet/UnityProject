using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using OldBlood.Code.Libaries.Generic;
using OldBlood.Code.Core.Server;
using UnityEngine;

namespace OldBlood.Code.Core.Server
{
    public class Server
    {

        public GenProperty<ServerConnectionManager> scm;
        public GenProperty<ServerWorldManager> swm;

        private Socket socket;
        private Thread thread;

        public Server()
        {
            Instance = this;
        }

        public void StartServer()
        {
            socket = CreateServerSocket();
            thread = new Thread(ServerUpdate);
            thread.Start();
            Debug.Log("Server running.");
        }

        public void ServerUpdate()
        {
            while(true)
            {
                Debug.Log("Listening");
                if(socket.IsBound)
                scm.Get.AcceptConnections(socket);
                Debug.Log("Listening2");
                swm.Get.ProgressWorlds();
                Debug.Log("Listening3");
                Thread.Sleep(30);
            }
        }

        static Socket CreateServerSocket()
        {
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 59580));
            newSocket.Listen(10);
            return newSocket;
        }

        private static Server instance;

        public static Server Instance
        {
            get
            {
                return instance;
            }
            private set
            {
                if (instance != null)
                {
                    instance.Stop();
                }
                instance = value;
            }
        }

        public void Stop()
        {
            socket.Close();
            thread.Abort();
            Debug.Log("Stopping server.");
        }
    }
}