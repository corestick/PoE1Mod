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


      // 챈터 노래 / 오라 제외
      if (__instance.IsAura || __instance.PhraseOrigin != null)
        return;

      // 정수주입 계열 제외 (선택)
      if (__instance.Params.AffectsStat == StatusEffect.ModifiedStat.MaxHealth ||
          __instance.Params.AffectsStat == StatusEffect.ModifiedStat.MaxStamina)
        return;

      // 🔥 최종 지속시간 덮어쓰기
      __result = Main.Settings.SoTDurationMinutes * 60f;
    }
  }
}