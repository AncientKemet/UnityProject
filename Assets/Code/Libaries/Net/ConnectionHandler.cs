﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace OldBlood.Code.Libaries.Net
{
    public class ConnectionHandler
    {
        private const int MAX_PACKETS_PROCEED_AT_ONCE = 10;

        private Socket socket;
        private PacketExecutor packetExecutor;
        private List<BasePacket> outgoingPackets = new List<BasePacket>();
        private int expectedNextLength;

        public ConnectionHandler(Socket socket, PacketExecutor packetExecutor) {
            this.socket = socket;
            this.packetExecutor = packetExecutor;
        }

        public void ReadAndExecute()
        {
            if (!socket.Connected)
            {
                return;
            }

            for (int i = 0; i < MAX_PACKETS_PROCEED_AT_ONCE; i++)
            {
                int available = socket.Available;

                if (available > 0)
                {
                    if (expectedNextLength == -1)
                    {
                        //ignore the two bytes for the future
                        available -= 2;

                        byte[] bytes = new byte[2];

                        socket.Receive(bytes);

                        ByteStream _in = new ByteStream(bytes);

                        expectedNextLength = _in.getUnsignedShort();
                    }

                    if (expectedNextLength != -1)
                    {
                        if (available > expectedNextLength)
                        {
                            //ignore the expected lenght for the future
                            available -= expectedNextLength;

                            byte[] bytes = new byte[expectedNextLength];

                            socket.Receive(bytes);

                            ByteStream _in = new ByteStream(bytes);

                            int opcode = _in.getUnsignedByte();

                            BasePacket packet = PacketManager.PacketForOpcode(opcode);

                            packet.Deserialize(_in);

                            packetExecutor.ExecutePacket(packet);
                        }
                    }
                }
            }
        }

        public void FlushOutPackets()
        {
            foreach (var packet in outgoingPackets)
            {
                ByteStream bytestream = new ByteStream();
                packet.Serialize(bytestream);

                //send
                socket.Send(bytestream.GetBuffer());
            }
            outgoingPackets.Clear();
        }

        public void SendPacket(BasePacket packet)
        {
            outgoingPackets.Add(packet);
        }
    }
}
