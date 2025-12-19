using HarmonyLib;
using UnityModManagerNet;
using System.Collections.Generic;

namespace EnablePerEncounterSpells
{
  [HarmonyPatch(typeof(CharacterStats), "HandleGameUtilitiesOnCombatEnd")]
  public static class CharacterStats_HandleCombatEnd_Patch
  {
    [HarmonyPostfix]
    public static void Postfix(CharacterStats __instance)
    {
      if (__instance == null)
        return;

      var abilities = Traverse
        .Create(__instance)
        .Field<IList<GenericAbility>>("m_abilities")
        .Value;

      if (abilities == null)
        return;

      foreach (var ability in abilities)
      {
        if (ability == null)
          continue;

        // ✅ Per Rest 능력만 대상
        if (ability.CooldownType != GenericAbility.CooldownMode.PerRest)
          continue;

        // 🔑 휴식과 동일하게: 쿨다운이 남아있는 동안 계속 복구
        while (ability.IsInCooldownAtMax || ability.UsesLeft() < ability.MaxCooldown)
        {
          ability.RestoreCooldown();

          // 안전장치 (이상 루프 방지)
          if (ability.UsesLeft() <= 0)
            break;
        }
      }
    }
  }
}