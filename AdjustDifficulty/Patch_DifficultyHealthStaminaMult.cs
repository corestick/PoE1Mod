using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace AdjustDifficulty
{
  [HarmonyPatch(typeof(CharacterStats), "get_DifficultyHealthStaminaMult")]
  public static class Patch_DifficultyHealthStaminaMult
  {
    public static void Postfix(CharacterStats __instance, ref float __result)
    {
      if (__instance.IsPartyMember || (!__instance.IsPartyMember && __instance.HasFactionSwapEffect()) || !GameState.Instance)
      {
        __result = 1f;
      }
      else
      {
        __result = Main.Settings.HealthStaminaMult;
      }
    }
  }
}