using OldBlood.Code.Libaries.Net;
using OldBlood.Code.Libaries.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Net
{
    public class NET : Monosingleton<NET>
    {
        [SerializeField]
        private string adress = "127.0.0.1";
        [SerializeField]
        private int port = 59580;

        private Socket socket;
        private ConnectionHandler conhan;

        void Start()
        {
            Server.KemetUnityServer.RunServer();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(IPAddress.Parse(adress), port);

            conhan = new ConnectionHandler(socket, new PlayerPacketExecutor());

            //Create auth packet
            AuthenticationPacket packet = new AuthenticationPacket();
            conhan.SendPacket(packet);

            if (socket.Connected)
            {
                Debug.Log("Connected to server.");
            }
            else
            {
                Debug.Log("Not connected to server.");
            }


        }

        void FixedUpdate()
        {
            if (conhan != null)
            {
                conhan.ReadAndExecute();
                conhan.FlushOutPackets();
            }
        }
    }
}
