using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DisableFriendlyFireAOE
{
  [HarmonyPatch(typeof(AttackAOE))]
  [HarmonyPatch("FindAoeTargets")]
  internal static class Patch_AttackAOE
  {
    private static void Postfix(
      ref List<GameObject> __result,
      AttackAOE __instance,
      GameObject caster)
    {
      if (__instance.ValidTargets != AttackBase.TargetType.All)
        return;

      if (__result == null || __result.Count == 0)
        return;

      var filtered = new List<GameObject>(__result.Count);

      foreach (var target in __result)
      {
        if (!target || !caster)
          continue;

        Faction tf = target.GetComponent<Faction>();
        Faction cf = caster.GetComponent<Faction>();

        if (!tf || !cf)
        {
          filtered.Add(target);
          continue;
        }

        // 혼란 상태여도 "원래 적"이면 유지
        AIController ai = GameUtilities.FindActiveAIController(target);
        if (ai != null)
        {
          Team original = ai.GetOriginalTeam();
          if (original != null)
          {
            var originalRel =
              original.GetRelationship(cf.CurrentTeam);

            if (originalRel == Faction.Relationship.Hostile)
            {
              filtered.Add(target);
              continue;
            }
          }
        }

        var relation =
          tf.CurrentTeam.GetRelationship(cf.CurrentTeam);

        // Friendly / Neutral 제거
        if (relation == Faction.Relationship.Friendly ||
            relation == Faction.Relationship.Neutral)
        {
          continue;
        }

        filtered.Add(target);
      }

      __result = filtered;
    }
  }
}
