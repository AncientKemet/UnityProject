using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Client.UI.Interfaces;
using Code.Code.Libaries.Net;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Shared.NET;
using Code.Libaries.Generic;
using Code.Libaries.Generic.Managers;
using UnityEngine;

namespace Code.Core.Client.Net
{
    public class ClientCommunicator : MonoSingleton<ClientCommunicator>
    {
        [SerializeField]
        private string adress = "127.0.0.1";

        private Socket socket;
        private ConnectionHandler conhan;

        public string[] opcodes = new string[256];

        void Start()
        {
            //Fill inspector opcode names
            PacketManager.PacketForOpcode(1);
            foreach (KeyValuePair<int, Type> pair in PacketManager.packetTypes)
            {
                opcodes[pair.Key] = pair.Value.Name;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(IPAddress.Parse(adress), NetworkConfig.I.port);

            conhan = new ConnectionHandler(socket, new PlayerPacketExecutor());
            
            if (socket.Connected)
            {
                Debug.Log("Connected to server.");
                UIContentManager.I.LoadInterfaces();
                LoginInterface.I.Show();
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

        public void SendToServer(BasePacket packet)
        {
            if (conhan != null && packet != null)
            {
                conhan.SendPacket(packet);
            }
        }
    }
}
