using System.Collections.Generic;

namespace Code.Core.Server.Model.Extensions.UnitExts
{
    
    public class UnitDisplay : UnitUpdateExt
    {
        private int _modelId = 1;
        
        public int ModelID
        {
            get { return _modelId; }
            set { _modelId = value;
                _wasUpdate = true;
            }
        }

        public override byte UpdateFlag()
        {
            return 0x02;
        }

        protected override void pSerializeState(Code.Libaries.Net.ByteStream packet)
        {
            packet.addByte((int) ModelID);
        }

        protected override void pSerializeUpdate(Code.Libaries.Net.ByteStream packet)
        {
            packet.addByte((int)ModelID);
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            _wasUpdate = true;
        }
    }
}
