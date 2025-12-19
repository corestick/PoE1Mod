using HarmonyLib;

[HarmonyPatch(typeof(StatusEffect), "get_Stackable")]
public static class Patch_StatusEffect_Stackable
{
  [HarmonyPostfix]
  public static void Postfix(StatusEffect __instance, ref bool __result)
  {
    if (__instance.AbilityType == GenericAbility.AbilityType.Equipment)
    {
      __result = true;
    }

    // if (__instance.AbilityType == GenericAbility.AbilityType.Ring)
    // {
    //   __result = true;
    // }
  }
}
