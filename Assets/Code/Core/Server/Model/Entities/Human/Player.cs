using Code.Code.Libaries.Net.Packets;
using Code.Core.Server.Model.Entities.Groups;
using Code.Core.Server.Model.Extensions.PlayerExtensions;
using Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers;
using Code.Core.Server.Model.Extensions.UnitExts;
using Code.Libaries.UnityExtensions;
using UnityEngine;

namespace Code.Core.Server.Model.Entities
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
                AddExt(_client);
            }
        }

        public PlayerInput PlayerInput { get; private set; }
        public ClientUI ClientUi { get; private set; }
        public PlayerChat Chat { get; private set; }
        public PlayerParty Party { get; set; }

        public override void Awake()
        {
            base.Awake();

            AddExt(ClientUi = new ClientUI());
            AddExt(PlayerInput = new PlayerInput());
            AddExt(Chat = new PlayerChat());

            //Server.Instance.swm.Get.Kemet.AddEntity(ServerMonoBehaviour.CreateInstance<Npc>());
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
            enterWorldPacket.Position = GetExt<UnitMovement>().Position;
            Client.ConnectionHandler.SendPacket(enterWorldPacket);
        }
    }
}

