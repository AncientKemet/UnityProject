using System.Collections.Generic;
using Code.Core.Shared.Content.Types;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Libaries.Generic.Managers
{
    public class ContentManager : SIAsset<ContentManager>
    {
        public List<Item> InventoryItems;
        public List<GameObject> Models;

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/ContentManager")]
        private static void SelectAsset()
        {
            Selection.activeObject = I;
        }
#endif
    }
}

