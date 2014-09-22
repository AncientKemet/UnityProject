using Code.Libaries.Generic;

#if UNITY_EDITOR
using UnityEditor;
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
