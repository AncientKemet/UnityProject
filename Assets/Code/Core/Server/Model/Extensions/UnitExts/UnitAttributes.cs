using Server.Model.Entities;

namespace Server.Model.Extensions.UnitExts
{
    public class UnitAttributes : EntityExtension
    {
        public float Strenght { get; set; }
        public float Dexterity { get; set; }
        public float Wisdom { get; set; }

        public float MaxHealth { get { return 100 + Strenght * 0.33f; } }
        public float MaxEnergy { get { return 100 + Dexterity * 0.33f; } }

        public float PhysicalDamage { get; private set; }
        public float MagicalDamage { get; private set; }

        public float Armor { get; private set; }
        public float MagicResist { get; private set; }

        public float MovementSpeed { get; private set; }
        public float Mobility { get; private set; }
        public float CooldownSpeed { get; private set; }

        public ServerUnit Unit { get; private set; }

        private int _updateTick = 0;

        public override void Progress()
        {
            _updateTick ++;

            if (_updateTick == 5)
            {
                UpdateAttributes();
                _updateTick = 0;
            }
        }

        public void UpdateAttributes()
        {
            Armor = 0;
            MagicResist = 0;

            PhysicalDamage = (100f + Strenght*0.33f) / 100f;
            MagicalDamage = (100f + Wisdom*0.33f) / 100f;

            MovementSpeed = (100f + Dexterity*0.15f) / 100f;
            Mobility = (100f + Dexterity*0.20f) / 100f;

            CooldownSpeed = (Dexterity*0.33f) / 100f;
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();
            Unit = entity as ServerUnit;
        }

    }
}
