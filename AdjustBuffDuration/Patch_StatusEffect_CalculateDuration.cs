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

      // 챈터 노래 / 오라 제외
      if (__instance.IsAura || __instance.PhraseOrigin != null)
        return;

      // 버프명 필터 음식
      var origin = __instance.Origin;
      if (origin != null)
      {
        string name = origin.name;

        // Clone 붙어도 startsWith는 그대로 먹힘
        if (name.StartsWith("Food_"))
        {
          Debug.Log($"[SoT][FILTER] Food blocked: {name}");
          return;
        }
      }

      //LogParams(__instance, "CHECKBUFF");

      // 치유 계열
      if (__instance.Params.AffectsStat == StatusEffect.ModifiedStat.Health ||
          __instance.Params.AffectsStat == StatusEffect.ModifiedStat.Stamina ||
          __instance.Params.AffectsStat == StatusEffect.ModifiedStat.HealthPercent ||
          __instance.Params.AffectsStat == StatusEffect.ModifiedStat.StaminaPercent)
        return;

      // 정수주입 계열 제외 (선택)
      if (__instance.Params.AffectsStat == StatusEffect.ModifiedStat.MaxHealth ||
          __instance.Params.AffectsStat == StatusEffect.ModifiedStat.MaxStamina)
        return;

      // 🔥 최종 지속시간 덮어쓰기
      __result = Main.Settings.SoTDurationMinutes * 60f;
    }

    static void LogParams(StatusEffect se, string tag)
    {
      var p = se.Params;
      var origin = se.Origin ? se.Origin.name : "NULL";

      Main.Mod.Logger.Log($"[{tag}] StatusEffect");
      Main.Mod.Logger.Log($" Origin               : {origin}");
      Main.Mod.Logger.Log($" AffectsStat          : {p.AffectsStat}");
      Main.Mod.Logger.Log($" Duration             : {p.Duration}");
      Main.Mod.Logger.Log($" DmgType              : {p.DmgType}");
      Main.Mod.Logger.Log($" Value                : {p.Value}");

      if (p.ConsumablePrefab != null)
        Main.Mod.Logger.Log($" ConsumablePrefab: {p.ConsumablePrefab.name}");

      if (p.AbilityPrefab != null)
        Main.Mod.Logger.Log($" AbilityPrefab   : {p.AbilityPrefab.name}");
    }
  }
}