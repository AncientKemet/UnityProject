using System;
using System.Net.Sockets;
using OldBlood.Code.Core.Server.Model;
using System.Collections.Generic;
using OldBlood.Code.Core.Server.Model.Extensions;
using OldBlood.Code.Core.Server.Model.Entities;
using UnityEngine;

namespace OldBlood.Code.Core.Server
{
    public class ServerConnectionManager
    {

        public void AcceptConnections(Socket socket)
        {
            socket.BeginAccept(new AsyncCallback(acceptCallback), socket);
        }

        public void acceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket newConnection = listener.EndAccept(ar);

            if (newConnection != null)
            {
                Debug.Log("new client!");
                ServerClient client = new ServerClient(newConnection);
                Server.Instance.swm.Get.Kemet.AddEntity(new Player(client));
            }
        }
    }
}

