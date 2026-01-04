using HarmonyLib;
using UnityEngine;

namespace StackableMod
{
  [HarmonyPatch(typeof(StatusEffect), "get_Stackable")]
  public static class Patch_StatusEffect_Stackable
  {
    [HarmonyPostfix]
    public static void Postfix(StatusEffect __instance, ref bool __result)
    {
      if (__result)
        return;

      if (Main.Settings.EnableForAbilities
        && __instance.Target != null
        && PartyHelper.IsPartyMember(__instance.Target))
      {
        if (__instance.AbilityType == GenericAbility.AbilityType.Ability
        || __instance.AbilityType == GenericAbility.AbilityType.Talent)
        {
          __result = true;
        }
      }

      if (Main.Settings.EnableForEquipment
        && __instance.Owner != null
        && PartyHelper.IsPartyMember(__instance.Owner))
      {
        if (__instance.AbilityType == GenericAbility.AbilityType.Equipment
          || __instance.AbilityType == GenericAbility.AbilityType.Ring)
        {
          __result = true;
        }
      }
    }
  }
}