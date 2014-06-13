using Code.Code.Libaries.Net;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Server.Model.Extensions.PlayerExtensions;
using Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers;
using Code.Core.Server.Model.Extensions.UnitExts;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Server.Net
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
                return;
            }

            if (packet is UIInterfaceEvent)
            {
                ClientUI ui = client.entity.GetExt<ClientUI>();
                if (ui != null)
                {
                    ui.OnUIEvent(packet as UIInterfaceEvent);
                }
                return;;
            }

            if (packet is WalkRequestPacket)
            {
                WalkRequestPacket update = packet as WalkRequestPacket;
                UnitMovement mov = client.Player.GetExt<UnitMovement>();
                if (mov != null)
                {
                    mov.WalkWay(update.DirecionVector);
                }
                return;
            }

            if (packet is InputEventPacket)
            {
                InputEventPacket inputEventPacket = packet as InputEventPacket;
                client.Player.PlayerInput.AddInput(inputEventPacket.type);
                return;
            }

            Debug.LogError("Unable to decode packet from Client: "+packet.GetType().Name);
        }
    }
}
