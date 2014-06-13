using Code.Code.Libaries.Net;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.UI.Interfaces;
using Code.Core.Client.Units;
using Code.Core.Client.Units.Extensions;
using Code.Core.Client.Units.Managed;
using Code.Libaries.Net.Packets.ForClient;
using UnityEngine;

namespace Code.Core.Client.Net
{
    public class PlayerPacketExecutor : PacketExecutor
    {
        protected override void aExecutePacket(BasePacket packet)
        {
            ClientCommunicator.Instance.PacketHistory.Add(packet.GetType().Name);
            if (packet is UIPacket)
            {
                UIPacket p = packet as UIPacket;
                if (p.type == UIPacket.UIPacketType.SEND_MESSAGE)
                {
                    if (ChatPanel.I != null)
                    ChatPanel.I.AddMessage(p.textData);
                }
            }
            else if (packet is EnterWorldPacket)
            {
                EnterWorldPacket p = packet as EnterWorldPacket;
                //load some world

                PlayerUnit.MyPlayerUnit = UnitManager.Instance[p.myUnitID];
                PlayerUnit.MyPlayerUnit.transform.position = p.Position;
            }
            else if (packet is UnitUpdatePacket)
            {
                UnitUpdatePacket p = packet as UnitUpdatePacket;
                UnitManager.Instance[p.UnitID].DecodeUnitUpdate(p);
            }
            else
            {
                Debug.LogError("Unknown packet type: " + packet.GetType());
            }
        }
    }
}
