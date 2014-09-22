using System.Collections.Generic;
using Client.UI.Interfaces;
using Code.Core.Client.UI;
using Code.Core.Client.UI.Interfaces;
using Code.Core.Client.UI.Interfaces.UpperLeft;
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
        public List<InterfaceManager.UIInterface> Interfaces; 
        
        public void LoadInterfaces()
        {
            Interfaces = new List<InterfaceManager.UIInterface>();
            foreach (var VARIABLE in FindObjectsOfType<InterfaceManager.UIInterface>())
            {
                Interfaces.Add(VARIABLE);
                VARIABLE.gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/UIContentManager/Load Interfaces")]
        private static void LoadInterfacesIntoAsset()
        {
            I.LoadInterfaces();
        }
#endif

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/UIContentManager")]
        private static void SelectAsset()
        {
            Selection.activeObject = I;
        }
#endif
    }
}

