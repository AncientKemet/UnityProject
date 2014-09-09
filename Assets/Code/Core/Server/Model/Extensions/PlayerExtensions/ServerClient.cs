using System.Net.Sockets;
using Code.Code.Libaries.Net;
using Server.Model.Entities.Human;
using Server.Net;

namespace Server.Model.Extensions.PlayerExtensions
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
