using System.Net.Sockets;
using Code.Code.Libaries.Net;
using Code.Core.Server.Model.Entities;
using Code.Core.Server.Net;

namespace Code.Core.Server.Model.Extensions.PlayerExtensions
{
    public class ServerClient : EntityExtension
    {

        private Socket socket;
        private Player player;

        private ConnectionHandler connectionHandler;

        public ConnectionHandler ConnectionHandler { get { return connectionHandler; } }

        public Player Player
        {
            get
            {
                if (player == null)
                {
                    if (entity is Player)
                        player = entity as Player;
                }
                return player;
            }
        }

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
