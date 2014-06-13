using UnityEngine;
namespace Code.Core.Server.Model.Extensions.UnitExts
{
    public class UnitCombat : UnitUpdateExt
    {

        public float Health { get; private set; }
        public float Energy { get; private set; }

        private int RegenTick = 0;
        const int RegenUpdateTick = 3;

        public override void Progress()
        {
            base.Progress();

            RegenTick++;

            if (RegenTick >= RegenUpdateTick)
            {   
                RegenTick = 0;

                Energy += 0.3f;
                Energy = Mathf.Clamp(Energy, 0, 100);
                Health += 0.01f;
                Health = Mathf.Clamp(Health, 0, 100);
                _wasUpdate = true;
            }
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Health = 100;
            Energy = 100;

            _wasUpdate = true;
        }

        public override byte UpdateFlag()
        {
            return 0x04;
        }

        protected override void pSerializeState(Code.Libaries.Net.ByteStream packet)
        {
            packet.addShort((int)Health);
            packet.addShort((int)Energy);
        }

        protected override void pSerializeUpdate(Code.Libaries.Net.ByteStream packet)
        {
            packet.addShort((int)Health);
            packet.addShort((int)Energy);
        }

        internal void ReduceEnergy(float speed)
        {
            Energy -= speed;
            Energy = Mathf.Clamp(Energy, 0, 100);
            _wasUpdate = true;
        }
    }
}
