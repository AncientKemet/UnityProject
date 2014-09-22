
#if SERVER
using Server.SQL;
using Server.Model.Extensions.UnitExts;

namespace Server.Model.Entities.Human
{
    public class Human : ServerUnit
    {

        [SQLSerialize]
        public UnitInventory Inventory;
        
        public override void Awake()
        {
            AddExt(Attributes = new UnitAttributes());
            AddExt(Anim = new UnitAnim());
            AddExt(Combat = new UnitCombat());
            AddExt(Inventory = new UnitInventory());

            Inventory.Width = 4;
            Inventory.Height = 4;

            base.Awake();
        }

    }
}

#endif
