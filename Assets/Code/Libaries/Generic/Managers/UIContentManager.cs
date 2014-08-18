using System.Collections.Generic;
using Code.Core.Shared.Content.Types;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Code.Libaries.Generic.Managers
{
    public class UIContentManager : SIAsset<UIContentManager>
    {
        public tk2dSlicedSprite ItemButtonBackGround;

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/UIContentManager")]
        private static void SelectAsset()
        {
            Selection.activeObject = I;
        }
#endif
    }
}

