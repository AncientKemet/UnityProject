
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Code.Core.Shared.Content.Types
{
    public class Npc : ContentItem
    {
        public string NpcName;

        public UnitVisual Visual;
    }
}

