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
    static void Postfix(
    StatusEffect __instance,
    GameObject target,
    bool ignoreTemporaryAdjustment,
    ref float __result)
    {
      // 지속시간 없는 효과
      if (__result <= 0f)
        return;

      // 버프 시전자
      var owner = __instance.Owner;
      if (owner == null)
        return;

      var ownerStats = owner.GetComponent<CharacterStats>();
      if (ownerStats == null || !ownerStats.IsPartyMember)
        return;

      // 대상 확인 (나 또는 파티원인가?)
      if (target == null)
        return;

      CharacterStats targetStats = target.GetComponent<CharacterStats>();
      if (targetStats == null || !targetStats.IsPartyMember)
        return;

      // 버프명 필터 음식
      var origin = __instance.Origin;
      if (origin != null)
      {
        string originName = origin.name;

        if (!BuffWhitelistManager.IsAllowed(originName))
        {
          if (!BuffWhitelistManager.IsFilterd(originName))
          {
            Main.LogParams($"[StatusEffect]");
            Main.LogParams($" Origin             : {origin}");
            Main.LogParams($" AffectsStat        : {__instance.Params.AffectsStat}");
            Main.LogParams($" Duration           : {__instance.Params.Duration}");
            Main.LogParams($" DmgType            : {__instance.Params.DmgType}");
            Main.LogParams($" Value              : {__instance.Params.Value}");
            Main.LogParams($" MaxRestCycles      : {__instance.Params.MaxRestCycles}");
          }

          return;
        }
        else
        {
          // 🔥 최종 지속시간 덮어쓰기
          __result = Main.Settings.BuffDurationMinutes * 60f;
        }
      }
    }
  }
}