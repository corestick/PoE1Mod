using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(StatusEffect), "get_Stackable")]
public static class Patch_StatusEffect_Stackable
{
  [HarmonyPostfix]
  public static void Postfix(StatusEffect __instance, ref bool __result)
  {
    // 기본 false 유지
    if (__result)
      return;

    if (__instance.Target != null &&
    PartyHelper.IsPartyMember(__instance.Target))
    {
      if (__instance.AbilityType == GenericAbility.AbilityType.Ability)
      {
        __result = true;
      }
    }

    if (__instance.Owner != null &&
    PartyHelper.IsPartyMember(__instance.Owner))
    {
      if (__instance.AbilityType == GenericAbility.AbilityType.Equipment)
      {
        __result = true;
      }
    }
  }
}
