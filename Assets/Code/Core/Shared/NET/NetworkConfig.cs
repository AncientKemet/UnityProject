using Code.Libaries.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Code.Core.Shared.NET
{
    public class NetworkConfig : SIAsset<NetworkConfig>
    {
        public int port = 54787;

#if UNITY_EDITOR
        [MenuItem("Kemet/Open/Network")]
        private static void SelectAsset()
        {
            Selection.activeObject = I;
        }
#endif
    }
}
