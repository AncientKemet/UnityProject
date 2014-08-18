using Code.Core.Server.Model.Entities;
using Code.Libaries.Generic.Managers;
using UnityEditor;
using UnityEngine;

namespace Code.Core.Shared.Content.Types
{
    public class Buff : ScriptableObject
    {
        private int _inContentManagerId = -1;

        public Texture2D Icon;
        
        [Range(0, 100)] public float Duration = 10f;

        [Multiline(5)]
        public string Description = "";

        public int InContentManagerId
        {
            get
            {
                if (_inContentManagerId == -1)
                {
                    _inContentManagerId = ContentManager.I.Buffs.IndexOf(this);
                }
                return _inContentManagerId;
            }
        }

#if UNITY_EDITOR
        public virtual void ProgressUnit(ServerUnit unit)
        { }

        public static void CreateBuff<T>() where T : Buff
        {
            var asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, "Assets/ReferencedData/Content/Buffs/" + typeof(T).Name + ".asset");
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
        }
#endif

    }
}
