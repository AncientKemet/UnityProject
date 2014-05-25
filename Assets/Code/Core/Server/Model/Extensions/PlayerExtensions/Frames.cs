using System;
using OldBlood.Code.Core.Server.Model.Entities;
using OldBlood.Code.Libaries.Net.Packets;

namespace OldBlood.Code.Core.Server.Model.Extensions.PlayerExtensions
{
    public static class Frames
    {
        public static void SendMessage(ServerClient c, string message)
        {
            UIPacket packet = new UIPacket();

            packet.type = UIPacket.UIPacketType.SEND_MESSAGE;
            packet.textData = message;

            c.ConnectionHandler.SendPacket(packet);
        }
    }
}

