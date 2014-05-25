using System;
using OldBlood.Code.Core.Server.Model.Extensions;
using OldBlood.Code.Libaries.Net.Packets;
using UnityEngine;

namespace OldBlood.Code.Core.Server.Model.Entities
{
    public class Player : Unit
    {

        public ServerClient client;

        public Player(ServerClient client)
        {
            AddExt(client);
            this.client = client;
        }

        public override void Progress()
        {
            base.Progress();
        }

        public void OnEnteredWorld(World world)
        {
            EnterWorldPacket enterWorldPacket = new EnterWorldPacket();
            enterWorldPacket.worldId = world.ID;
            enterWorldPacket.Position = new Vector3( 50,50,50);
            client.ConnectionHandler.SendPacket(enterWorldPacket);
        }
    }
}

