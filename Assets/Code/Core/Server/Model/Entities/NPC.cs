
using Code.Core.Server.Model.Extensions.UnitExts;
using UnityEngine;

namespace Code.Core.Server.Model.Entities
{
    public class Npc : ServerUnit
    {

        private UnitMovement unitMovement;
        private ServerUnit _followingServerUnit;

        public override void Awake()
        {
            base.Awake();
            unitMovement = GetExt<UnitMovement>();
        }

        public override void Progress()
        {
            if (_followingServerUnit == null)
            {
                foreach (var unit in CurrentBranch.ObjectsVisible)
                {
                    if (unit is Player)
                        _followingServerUnit = unit as Player;
                }
            }
            else
            {
                Vector2 otherPos = _followingServerUnit.GetPosition();
                float distance = Vector2.Distance(GetPosition(), otherPos);
                if (distance > 5f)
                {
                    unitMovement.WalkTo(_followingServerUnit.GetExt<UnitMovement>().Position);
                }
            }
            base.Progress();
        }
    }
}

