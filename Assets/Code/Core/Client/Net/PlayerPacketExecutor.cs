using OldBlood.Code.Libaries.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldBlood.Code.Core.Client.Net
{
    public class PlayerPacketExecutor : PacketExecutor
    {
        protected override void aExecutePacket(BasePacket packet)
        {
            Debug.Log("Player recieved packet.");
        }
    }
}
