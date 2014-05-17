using OldBlood.Code.Core.Server.Model;
using OldBlood.Code.Core.Server.Net;
using OldBlood.Code.Libaries.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace OldBlood.Code.Core.Server.Model.Extensions
{
    public class ServerClient : EntityExtension
    {

        private Socket socket;

        private ConnectionHandler connectionHandler;

        public ConnectionHandler ConnectionHandler { get { return connectionHandler; } }

        public ServerClient(Socket _socket)
        {
            this.socket = _socket;
            connectionHandler = new ConnectionHandler(socket, new ClientPacketExecutor(this));
        }

        public override void Progress()
        {
            connectionHandler.ReadAndExecute();
            connectionHandler.FlushOutPackets();
        }

    }
}
