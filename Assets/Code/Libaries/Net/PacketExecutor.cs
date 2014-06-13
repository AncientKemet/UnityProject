using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Code.Code.Libaries.Net
{
    public abstract class PacketExecutor
    {

        protected abstract void aExecutePacket(BasePacket packet);

        public void ExecutePacket(BasePacket packet)
        {
            aExecutePacket(packet);
        }
    }
}
