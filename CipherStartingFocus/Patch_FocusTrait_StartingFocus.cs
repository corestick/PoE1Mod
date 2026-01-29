using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace CipherStartingFocus
{
  [HarmonyPatch(typeof(FocusTrait))]
  [HarmonyPatch("get_StartingFocus")]
  public static class Patch_FocusTrait_StartingFocus
  {
    // Token: 0x06000074 RID: 116 RVA: 0x000053CC File Offset: 0x000035CC
    private static void Postfix(ref float __result, FocusTrait __instance)
    {
      float baseMaxFocus = __instance.MaxFocus - __instance.MaxFocusBonus;

      __result = baseMaxFocus * (Main.Settings.StartFocusPercent / 100);
      __result += __instance.MaxFocusBonus;
    }
  }
}