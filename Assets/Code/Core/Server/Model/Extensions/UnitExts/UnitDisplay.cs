using UnityEngine;

namespace OldBlood.Code.Core.Server.Model.Extensions.UnitExts
{

    public enum UnitDisplayType
    {
        Invisible = 0,
        Human = 1
    }

    public class UnitDisplay : UnitUpdateExt
    {
        private UnitDisplayType _displayType;

        public UnitDisplayType DisplayType
        {
            get { return _displayType; }
            set { _displayType = value;
                _wasUpdate = true;
            }
        }


        public override byte UpdateFlag()
        {
            return 0x02;
        }

        protected override void pSerializeState(Libaries.Net.ByteStream packet)
        {
            packet.addByte((int) DisplayType);
        }

        protected override void pSerializeUpdate(Libaries.Net.ByteStream packet)
        {
            packet.addByte((int)DisplayType);
        }
    }
}
