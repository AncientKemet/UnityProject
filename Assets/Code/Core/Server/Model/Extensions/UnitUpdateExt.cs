using OldBlood.Code.Core.Server.Model;
using OldBlood.Code.Core.Server.Net;
using OldBlood.Code.Libaries.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OldBlood.Code.Core.Server.Model.Extensions
{
    
    public abstract class UnitUpdateExt : EntityExtension
    {

        protected bool _wasUpdate;

        public bool WasUpdate()
        {
            return _wasUpdate;
        }

        /// <summary>
        /// Used when composing final unit update.
        /// 0x01 - movement
        /// 0x02 - display
        /// ...
        /// </summary>
        /// <returns>Byte flag</returns>
        public abstract byte UpdateFlag();

        protected abstract void pSerializeState(ByteStream packet);
        protected abstract void pSerializeUpdate(ByteStream packet);

        public override void Progress()
        {
            _wasUpdate = false;
        }

        /// <summary>
        /// Serializes current state of this extension into packet.
        /// Use: Client has never seen an object, so we need to send him position, even if it wasn't updated recently.
        /// </summary>
        /// <param name="packet"></param>
        public void SerializeState(ByteStream packet)
        {
            pSerializeState(packet);
        }

        /// <summary>
        /// Serializes this extension lastest update into packet.
        /// </summary>
        /// <param name="packet"></param>
        public void SerializeUpdate(ByteStream packet)
        {
            pSerializeUpdate(packet);
        }

    }
}
