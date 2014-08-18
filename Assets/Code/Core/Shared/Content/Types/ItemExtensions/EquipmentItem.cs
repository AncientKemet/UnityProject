#if UNITY_EDITOR
#endif
using System;

namespace Code.Core.Shared.Content.Types.ItemExtensions
{
    [Serializable]
    public class EquipmentItem : ItemExtension
    {
        public enum Type 
        {
            LeftHand,
            RightHand,
            TwoHand,
            Helm,
            ChestArmor,
            Necklace,
            Back,
            Wirsts
        }

        public Type EquipType;

        public bool CanBeStoredInInventory = true;
    }
}
