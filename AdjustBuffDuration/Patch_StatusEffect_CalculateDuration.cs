using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace AdjustBuffDuration
{
  [HarmonyPatch(typeof(StatusEffect), "CalculateDuration")]
  public static class Patch_StatusEffect_CalculateDuration
  {
    static bool Prefix(
    StatusEffect __instance,
    GameObject target,
    bool ignoreTemporaryAdjustment,
    ref float __result)
    {
      if (!__instance.m_needsDurationCalculated)
        return true;

      if (__instance.Origin == null || __instance.AbilityOrigin == null)
        return true;

      if (__instance.Params.Duration <= 0f && __instance.m_durationOverride <= 0f)
        return true;

      if (ShouldApply(__instance, target))
      {
        // Main.LogEffect(__instance);
        __instance.Duration = Main.Settings.BuffDurationMinutes * 60f;
        __instance.m_durationOverride = Main.Settings.BuffDurationMinutes * 60f;
      }

      return true;
    }

    private static bool ShouldApply(StatusEffect __instance, GameObject target)
    {
      // 시전자
      var owner = __instance.Owner;
      if (owner == null)
        return false;

      var ownerStats = owner.GetComponent<CharacterStats>();
      if (ownerStats == null || !ownerStats.IsPartyMember)
        return false;

      // 대상
      var holder = target;
      if (holder == null)
        return false;

      var targetStats = holder.GetComponent<CharacterStats>();
      if (targetStats == null || !targetStats.IsPartyMember)
        return false;

      // 대상이 우리 편이 아니면
      if (targetStats.HasFactionSwapEffect())
        return false;

      var origin = __instance.Origin;
      if (origin == null)
        return false;

      string originName = origin.name;

      if (!BuffWhitelistManager.IsAllowed(originName))
      {
        if (!BuffWhitelistManager.IsFilterd(originName) && !string.IsNullOrEmpty(originName))
        {
          Main.LogEffect(__instance);
        }
        return false;
      }

      return true;
    }
  }
}