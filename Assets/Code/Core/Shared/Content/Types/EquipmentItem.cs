using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Code.Core.Shared.Content.Types;

namespace Code.Core.Shared.Content.Types
{
    [Serializable]
    public class EquipmentItem : Item
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
    }
}
