using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldBlood.Code.Libaries.Net.Packets
{
    class AuthenticationPacket : BasePacket
    {

        public string text = "bad";

        protected override int GetOpCode()
        {
            return 1;
        }

        protected override void enSerialize(ByteStream bytestream)
        {
            bytestream.addString("Lolol");
        }

        protected override void deSerialize(ByteStream bytestream)
        {
            text = bytestream.getString();
        }
    }
}
