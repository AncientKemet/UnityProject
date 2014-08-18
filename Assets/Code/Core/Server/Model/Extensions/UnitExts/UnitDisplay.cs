using System.Collections.Generic;
using Code.Core.Server.Model.Entities;
using Code.Core.Shared.Content;
using Code.Core.Shared.Content.Types;

namespace Code.Core.Server.Model.Extensions.UnitExts
{
    
    public class UnitDisplay : UnitUpdateExt
    {
        private bool IsItem { get { return _item != null; } }

        private int _modelId = 1;
        private Item _item;
        private bool _destroy;
        private ServerUnit _pickupingUnit;

        public int ModelID
        {
            get { return _modelId; }
            set { _modelId = value;
                _wasUpdate = true;
            }
        }

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                ModelID = _item.InContentManagerIndex;
            }
        }

        public float Size { get; set; }

        public bool Destroy
        {
            get { return _destroy; }
            private set
            {
                _destroy = value;
                _wasUpdate = true;
            }
        }

        public ServerUnit PickupingUnit
        {
            get { return _pickupingUnit; }
            set
            {
                _pickupingUnit = value;
                Destroy = true;
            }
        }

        public override byte UpdateFlag()
        {
            return 0x02;
        }

        protected override void pSerializeState(Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(IsItem, false);
            packet.addByte(ModelID);
        }

        protected override void pSerializeUpdate(Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(IsItem, Destroy);
            packet.addByte(ModelID);

            if (Destroy)
            {
                packet.addFlag(PickupingUnit != null);

                if (PickupingUnit != null)
                {
                    packet.addShort(PickupingUnit.ID);
                }

                Destroy = false;
                UnityEngine.Object.Destroy(entity.gameObject);
            }
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Size = 1f;
            _wasUpdate = true;
        }
    }
}
