using Code.Libaries.Generic.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using Code.Core.Client.UI;
using Code.Core.Server.Model.Entities;

using UnityEngine;

namespace Code.Core.Shared.Content.Types
{
    public abstract class Spell : ScriptableObject
    {
        private int _inContentManagerId = -1;
        
        public Texture2D Icon;

        public bool HasEnergyCost { get { return EnergyCost > 1f; } }

        [Range(0,100)]
        public float EnergyCost = 0f;

        [Range(0,5f)]
        public float MaxDuration = 1f;

        [Range(0,5f)]
        public float ActivableDuration = 0.5f;

        [Multiline(5)]
        public string Description = "";
        
        public int InContentManagerId
        {
            get
            {
                if (_inContentManagerId == -1)
                {
                    _inContentManagerId = ContentManager.I.Spells.IndexOf(this);
                }
                return _inContentManagerId;
            }
        }

#if UNITY_EDITOR

        public void StartCasting(ServerUnit unit)
        {
            Player p = unit as Player;

            if (p != null)
            {
                p.ClientUi.ShowControl(InterfaceType.ActionBars, 4);
            }

            OnStartCasting(unit);
        }

        public void FinishCasting(ServerUnit unit, float strenght)
        {
            strenght = Mathf.Clamp01(strenght);

            Player p = unit as Player;

            if (p != null)
            {
                p.ClientUi.HideControl(InterfaceType.ActionBars, 4);
            }

            OnFinishCasting(unit, strenght);
        }

        public void StrenghtChanged(ServerUnit unit, float strenght)
        {
            strenght = Mathf.Clamp01(strenght);

            Player p = unit as Player;

            if (p != null)
            {
                List<float> floats = new List<float>();

                floats.Add(strenght);

                p.ClientUi.SetControlValues(InterfaceType.ActionBars, 4, floats);
            }

            OnStrenghtChanged(unit, strenght);
        }

        public abstract void OnStartCasting(ServerUnit unit);
        public abstract void OnFinishCasting(ServerUnit unit, float strenght);
        public abstract void OnStrenghtChanged(ServerUnit unit, float strenght);

        public static void CreateSpell<T>() where T : Spell
        {
            var asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, "Assets/ReferencedData/Content/Spells/"+ typeof(T).Name + ".asset");
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
        }
#endif

    }
}
