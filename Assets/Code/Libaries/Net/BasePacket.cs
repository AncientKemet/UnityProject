using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OldBlood.Code.Libaries.Net
{

    public static class PacketManager 
    {
        private static List<Type> packetTypes = new List<Type>(256);
        private static bool _packetsWereLoaded;

        public static BasePacket PacketForOpcode(int opcode)
        {
            if (!_packetsWereLoaded) {
                _packetsWereLoaded = true;
                Type BasePacketType = typeof(BasePacket);
                foreach (var type in Assembly.GetAssembly(typeof(PacketManager)).GetTypes())
                {
                    if(BasePacketType.IsAssignableFrom(type))
                    {
                        object intance = Activator.CreateInstance(type);
                        int _opcode = (int)type.GetMethod("GetOpCode", BindingFlags.NonPublic).Invoke(intance, null);
                        packetTypes[_opcode] = type;
                    }
                }
            }

            BasePacket packetInstance = (BasePacket)Activator.CreateInstance(packetTypes[opcode]);
            return packetInstance;
        }
    }

    public abstract class BasePacket
    {
        protected abstract int GetOpCode();

        protected abstract void enSerialize(ByteStream bytestream);
        protected abstract void deSerialize(ByteStream bytestream);

        public void Deserialize(ByteStream bytestream)
        {
            deSerialize(bytestream);
        }

        public void Serialize(ByteStream bytestream) 
        {
            //add empty short for the size of packet
            bytestream.addShort(0);

            //add opcode
            bytestream.addByte(GetOpCode());

            //abstract serializing
            enSerialize(bytestream);

            //now we add the actual size of packet
            //set offset back
            bytestream.Offset = 0;

            //add the size, but with -2 cause its gonna be there always
            bytestream.addShort(bytestream.Length - 2);

            //Done :)
        }
    }
}
