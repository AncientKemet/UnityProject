using OldBlood.Code.Core.Client.UI.Interfaces;
using OldBlood.Code.Core.Client.Units.Extensions;
using OldBlood.Code.Libaries.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OldBlood.Code.Libaries.Net.Packets;

namespace OldBlood.Code.Core.Client.Net
{
    public class PlayerPacketExecutor : PacketExecutor
    {
        protected override void aExecutePacket(BasePacket packet)
        {
            if (packet is UIPacket)
            {
                UIPacket p = packet as UIPacket;
                if (p.type == UIPacket.UIPacketType.SEND_MESSAGE)
                {
                    ChatPanel.I.AddMessage(p.textData);
                }
            }
            else if (packet is EnterWorldPacket)
            {
                EnterWorldPacket p = packet as EnterWorldPacket;
                //load some world

                Player.MyPlayer.transform.position = p.Position;
            }
            else
            {
                Debug.LogError("Unknown packet type: " + packet.GetType());
            }
        }
    }
}
