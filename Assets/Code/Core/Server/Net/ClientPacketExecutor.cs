using OldBlood.Code.Core.Server.Model.Extensions;
using OldBlood.Code.Libaries.Net;
using OldBlood.Code.Libaries.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OldBlood.Code.Core.Server.Model.Extensions.PlayerExtensions;

namespace OldBlood.Code.Core.Server.Net
{
    public class ClientPacketExecutor : PacketExecutor
    {
        private ServerClient client;

        public ClientPacketExecutor(ServerClient client) {
            this.client = client;
        }

        protected override void aExecutePacket(BasePacket packet)
        {
            if(packet is AuthenticationPacket)
            {
                Frames.SendMessage(client, "Welcome to Ancient Kemet!");
            }
        }
    }
}
