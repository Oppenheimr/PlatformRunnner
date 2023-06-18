#if INVECTOR_BASIC || INVECTOR_MELEE || INVECTOR_SHOOTER || INVECTOR_AI_TEMPLATE
using Invector;
#endif

namespace UnityUtils.Extensions
{
    public static class DamageExtensions
    {
#if (INVECTOR_BASIC || INVECTOR_MELEE || INVECTOR_SHOOTER || INVECTOR_AI_TEMPLATE) && MFPS
        public static vDamage ConvertDamage(this DamageData damageData) => new vDamage()
        {
            damageValue = damageData.Damage,
        };
        
        public static DamageData ConvertDamage(this vDamage damage) => new DamageData()
        {
            Damage = (int)damage.damageValue,
        };
#endif
        
#if MFPS
        public static string GetWeaponName(this DamageData damageData) =>
            bl_GameData.Instance.GetWeapon(damageData.GunID).Name;

        public static Team GetTeam(this DamageData damageData) =>
            damageData.MFPSActor.Team;
        
        public static bool FromBot(this DamageData damageData) =>
            !damageData.MFPSActor.isRealPlayer;
#endif
    }
}