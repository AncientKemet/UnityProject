using Code.Code.Libaries.Net;

namespace Code.Libaries.Net.Packets.ForClient
{
    public class UnitUpdatePacket : BasePacket
    {

        public ByteStream SubPacketData = new ByteStream();
        public int UnitID;

        #region implemented abstract members of BasePacket

        protected override int GetOpCode()
        {
            return 20;
        }

        protected override void enSerialize(ByteStream bytestream)
        {
            bytestream.addShort(UnitID);
            bytestream.addShort(SubPacketData.Length);
            bytestream.addBytes(SubPacketData.GetBuffer());
        }

        protected override void deSerialize(ByteStream bytestream)
        {
            UnitID = bytestream.getUnsignedShort();
            int lenght = bytestream.getUnsignedShort();
            SubPacketData.addBytes(bytestream.getSubBuffer(lenght));
        }

        #endregion
    }
}

