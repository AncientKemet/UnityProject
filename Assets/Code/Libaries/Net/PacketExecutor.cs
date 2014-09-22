using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Code.Code.Libaries.Net
{
    public abstract class PacketExecutor
    {

        protected abstract void aExecutePacket(BasePacket packet);

        public void ExecutePacket(BasePacket packet)
        {
            try
            {
                aExecutePacket(packet);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
