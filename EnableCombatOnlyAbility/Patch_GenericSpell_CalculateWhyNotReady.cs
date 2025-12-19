using HarmonyLib;
using UnityModManagerNet;
using System.ComponentModel;

namespace EnableCombatOnlyAbility
{
  [HarmonyPatch(typeof(GenericSpell), "CalculateWhyNotReady")]
  public static class GenericSpell_NoCombatRestriction
  {
    static void Prefix(GenericSpell __instance, ref bool __state)
    {
      __state = __instance.CombatOnly;
      __instance.CombatOnly = false;
    }

    static void Postfix(GenericSpell __instance, bool __state)
    {
      __instance.CombatOnly = __state;
    }
  }

}