

using Code.Core.Shared.Content;
using Code.Core.Shared.Content.Types;
using Server.Model.Entities;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace ReferencedData.Content.Spells.Codes
{
    public class MeleeSpell : Spell
    {

        public string PowerAnim = "LeftHandSlashPower";
        public string AttackAnim = "LeftHandSlashAttack";

        public float BaseDamage = 0;
        public float StrenghtRatio = 1f;
        public float DexterityRatio = 1f;

#if SERVER

        public override void OnFinishCasting(ServerUnit unit, float strenght)
        {
            unit.Anim.ActionAnimation = AttackAnim;
        }

        public override void OnStartCasting(ServerUnit unit)
        {
            unit.Anim.ActionAnimation = PowerAnim;
        }

        public override void OnStrenghtChanged(ServerUnit unit, float strenght)
        {
            
        }
#if UNITY_EDITOR
        [MenuItem("Kemet/Create/Spell/Test")]
        public static void CreateTest()
        {
            CreateSpell<MeleeSpell>();
        }
#endif
#endif
    }
}
