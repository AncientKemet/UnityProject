using Code.Code.Libaries.Net;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.UI.Interfaces.UpperLeft;
using Code.Core.Server.Model.Entities;
using Code.Core.Server.Model.Extensions.PlayerExtensions;
using Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers;
using Code.Core.Server.Model.Extensions.UnitExts;
using Code.Libaries.Net.Packets.ForServer;
using Code.Libaries.Net.Packets.InGame;
using UnityEngine;

namespace Code.Core.Server.Net
{
    public class ClientPacketExecutor : PacketExecutor
    {
        private ServerClient client;

        public ClientPacketExecutor(ServerClient client)
        {
            this.client = client;
        }

        protected override void aExecutePacket(BasePacket packet)
        {

            if (packet is AuthenticationPacket)
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
                return; ;
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

            if (packet is TargetUpdatePacket)
            {
                TargetUpdatePacket p = packet as TargetUpdatePacket;

                if (client.Player.Focus.FocusedUnit != null)
                    if (client.Player.Focus.FocusedUnit.ID != p.UnitId)
                    {
                        client.Player.Focus.FocusedUnit.Focus.PlayersThatSelectedThisUnit.Remove(client.Player);
                    }

                if (p.UnitId != -1)
                {
                    ServerUnit selectedUnit = client.Player.CurrentWorld[p.UnitId];

                    client.Player.Focus.FocusedUnit = selectedUnit;
                    client.Player.Anim.LookingAt = selectedUnit;

                    if (selectedUnit != null)
                    {
                        selectedUnit.Focus.PlayersThatSelectedThisUnit.Add(client.Player);
                        
                        UnitSelectionPacketData data = new UnitSelectionPacketData();

                        data.HasCombat = selectedUnit.Combat != null;
                        data.HasAttributes = selectedUnit.Attributes != null;

                        data.UnitName = selectedUnit.name;

                        if (data.HasAttributes)
                        {
                            data.Armor = selectedUnit.Attributes.Armor;
                            data.MagicResist = selectedUnit.Attributes.MagicResist;
                            data.CooldownSpeed = selectedUnit.Attributes.CooldownSpeed;
                            data.Armor = selectedUnit.Attributes.Armor;
                            data.Mobility = selectedUnit.Attributes.Mobility;
                            data.MovementSpeed = selectedUnit.Attributes.MovementSpeed;
                        }

                        client.ConnectionHandler.SendPacket(data);
                    }
                }
                return;
            }

            if (packet is UnitActionPacket)
            {
                UnitActionPacket p = packet as UnitActionPacket;
                client.Player.Actions.DoAction(p.UnitId, p.ActionName);

                return;
            }

            if (packet is ChatPacket)
            {
                ChatPacket p = packet as ChatPacket;
                client.Player.Chat.HandlePacket(p);
                return;
            }


            Debug.LogError("Unable to decode packet from Client: " + packet.GetType().Name);
        }
    }
}
