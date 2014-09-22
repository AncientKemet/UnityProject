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
        public List<Item> Items;
        public List<GameObject> Models;
        public List<Spell> Spells;
        public List<Buff> Buffs;
        public List<GameObject> Effects;

        private void OnEnable()
        {
            foreach (var item in Items)
            {
                if(item != null)
                if (item.GUID == null) { }
            }
        }

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/ContentManager")]
        private static void SelectAsset()
        {
            Selection.activeObject = I;
        }
#endif

        public void CreateEffect(int i, Vector3 position)
        {
            Instantiate(Effects[i], position, Quaternion.identity);
        }
    }
}

