using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Code.Code.Libaries.Net.Packets
{
    class AuthenticationPacket : BasePacket
    {

        public string Username { get; set; }
        public string Password { get; set; }

        protected override int GetOpCode()
        {
            return 1;
        }

        protected override void enSerialize(ByteStream bytestream)
        {
            bytestream.addString(Username);
            bytestream.addString(Password);
        }

        protected override void deSerialize(ByteStream bytestream)
        {
            Username = bytestream.getString();
            Password = bytestream.getString();
        }
    }
}
