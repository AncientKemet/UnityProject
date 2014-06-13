using System.Net;
using System.Net.Sockets;
using System.Threading;
using Code.Core.Shared.NET;
using Code.Libaries.Generic;
using UnityEngine;

namespace Code.Core.Server
{
    public class Server
    {

        public GenProperty<ServerConnectionManager> scm = new GenProperty<ServerConnectionManager>();
        public GenProperty<ServerWorldManager> swm = new GenProperty<ServerWorldManager>();

        private Socket socket;

        public Server()
        {
            Instance = this;
        }

        public void StartServer()
        {
            socket = CreateServerSocket();
            Debug.Log("Server running.");
        }

        public void ServerUpdate()
        {
            scm.Get.AcceptConnections(socket);
            swm.Get.ProgressWorlds();
        }

        static Socket CreateServerSocket()
        {
            Socket newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newSocket.Blocking = false;
            newSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), NetworkConfig.I.port));
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
            Debug.Log("Stopping server.");
        }
    }
}