using Server.Model.Extensions.UnitExts;

namespace Server.Model.Entities.Human
{
    public class Human : ServerUnit
    {

        public override void Awake()
        {
            AddExt(Attributes = new UnitAttributes());
            AddExt(Anim = new UnitAnim());
            AddExt(Combat = new UnitCombat());
            base.Awake();
        }

    }
}

