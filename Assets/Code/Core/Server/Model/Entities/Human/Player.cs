using Code.Libaries.Generic.Managers;
#if SERVER
using Server.SQL;
using Code.Code.Libaries.Net.Packets;
using Server.Model.Entities.Groups;
using Server.Model.Extensions.PlayerExtensions;
using Server.Model.Extensions.PlayerExtensions.UIHelpers;
using Server.Model.Extensions.UnitExts;

namespace Server.Model.Entities.Human
{
    public class Player : Human
    {
        private ServerClient _client;

        public ServerClient Client
        {
            get { return _client; }
            set
            {
                _client = value;
                AddExt(value);
            }
        }

        public PlayerInput PlayerInput { get; private set; }
        public ClientUI ClientUi { get; private set; }
        public PlayerChat Chat { get; private set; }
        public PlayerParty Party { get; set; }
        public string Password { get; set; }

        public override void Awake()
        {
            base.Awake();

            AddExt(ClientUi = new ClientUI());
            AddExt(PlayerInput = new PlayerInput());
            AddExt(Chat = new PlayerChat());

            Inventory.AccesType = UnitInventory.AccessType.ONLY_THIS_UNIT;
            Inventory.ListeningPlayers.Add(this);
        }

        public override void Progress()
        {
            base.Progress();
        }

        public void OnEnteredWorld(World world)
        {
            EnterWorldPacket enterWorldPacket = new EnterWorldPacket();

            enterWorldPacket.worldId = world.ID;
            enterWorldPacket.myUnitID = ID;
            enterWorldPacket.Position = Movement.Position;

            Client.ConnectionHandler.SendPacket(enterWorldPacket);
        }
    }
}

#endif
