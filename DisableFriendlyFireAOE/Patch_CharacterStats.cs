using HarmonyLib;
using UnityEngine;

namespace DisableFriendlyFireAOE
{
  [HarmonyPatch(typeof(CharacterStats))]
  [HarmonyPatch("ComputeHitAdjustment")]
  internal static class Patch_CharacterStats
  {
    private static void Postfix(
      CharacterStats __instance,
      CharacterStats enemyStats,
      DamageInfo damage)
    {
      if (__instance == null || enemyStats == null || damage == null)
        return;

      GameObject attacker = __instance.gameObject;
      GameObject target = enemyStats.gameObject;

      if (!attacker || !target)
        return;

      Faction af = attacker.GetComponent<Faction>();
      Faction tf = target.GetComponent<Faction>();

      if (!af || !tf)
        return;

      Faction.Relationship relation =
        tf.CurrentTeam.GetRelationship(af.CurrentTeam);

      if (relation == Faction.Relationship.Friendly ||
          relation == Faction.Relationship.Neutral)
      {
        damage.IsMiss = true;
        damage.IsCriticalHit = false;
        damage.IsGraze = false;
        damage.Interrupts = false;
        damage.IsKillingBlow = false;
        damage.DamageMult(0f);
      }
    }
  }
}
