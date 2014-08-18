
using Code.Core.Server.Model.Extensions.UnitExts;
using UnityEngine;

namespace Code.Core.Server.Model.Entities
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

