using System;
using System.Collections;
using System.Net.Sockets;
using Code.Core.Server.Model.Entities;
using Code.Core.Server.Model.Extensions.PlayerExtensions;
using Code.Libaries.UnityExtensions;
using UnityEngine;

namespace Code.Core.Server
{
    public class ServerConnectionManager
    {

        public void AcceptConnections(Socket socket)
        {
            if(socket != null)
                socket.BeginAccept(new AsyncCallback(acceptCallback), socket);
        }

        public void acceptCallback(IAsyncResult ar)
        {
            var listener = (Socket)ar.AsyncState;
            var newConnection = listener.EndAccept(ar);

            if (newConnection != null)
            {
                var client = new ServerClient(newConnection);

                Action actionToRunOnUnityThread = delegate
                {
                    var player = ServerMonoBehaviour.CreateInstance<Player>();

                    player.Client = client;

                    Server.Instance.swm.Get.Kemet.AddEntity(player);
                };

                ServerSingleton.StuffToRunOnUnityThread.AddFirst(actionToRunOnUnityThread);
            }
        }

        
    }
}

