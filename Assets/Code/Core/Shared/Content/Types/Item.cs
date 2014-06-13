using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Code.Core.Shared.Content.Types
{
    [Serializable]
    public class Item : ContentItem
    {
        public string Name;

        public Texture2D ItemIcon;

        [Range(0f, 20f)]
        public float Weight;

        public bool Tradable = true;
        
        public bool Stackable = false;

        public int MaxStacks = 1;

        public GameObject Model;
    }

}
