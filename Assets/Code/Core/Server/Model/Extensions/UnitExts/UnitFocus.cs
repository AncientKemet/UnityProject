using System.Collections.Generic;
using Server.Model.Entities;
using Server.Model.Entities.Human;

namespace Server.Model.Extensions.UnitExts
{
    public class UnitFocus : EntityExtension
    {
        public List<Player> PlayersThatSelectedThisUnit = new List<Player>(5);

        private ServerUnit _focusedUnit { get; set; }

        public ServerUnit FocusedUnit 
        { 
            get 
            {
                return _focusedUnit;
            }
            set 
            {
                _focusedUnit = value;
                if (entity is Human) 
                {
                    (entity as Human).Anim.LookingAt = value;
                }
            }
        }

        public override void Progress()
        {
        }

    }
}
