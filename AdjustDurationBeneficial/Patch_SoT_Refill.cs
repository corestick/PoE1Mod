using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace AdjustDurationBeneficial
{
  // ─────────────────────────────────────────────
  // Thread-local SoT context
  // ─────────────────────────────────────────────
  static class SoTContext
  {
    [ThreadStatic] public static int Depth;
    [ThreadStatic] public static HashSet<StatusEffect> Touched;

    public static bool Active => Depth > 0;
  }

  // ─────────────────────────────────────────────
  // StatusEffect private fields (your build confirmed these names)
  // ─────────────────────────────────────────────
  static class SEPriv
  {
    public static readonly AccessTools.FieldRef<StatusEffect, float> Duration =
      AccessTools.FieldRefAccess<StatusEffect, float>("<m_duration>k__BackingField");

    public static readonly AccessTools.FieldRef<StatusEffect, float> TimeActive =
      AccessTools.FieldRefAccess<StatusEffect, float>("<m_timeActive>k__BackingField");

    public static readonly AccessTools.FieldRef<StatusEffect, float> IntervalTimer =
      AccessTools.FieldRefAccess<StatusEffect, float>("<m_intervalTimer>k__BackingField");

    public static readonly AccessTools.FieldRef<StatusEffect, float> TempAdj =
      AccessTools.FieldRefAccess<StatusEffect, float>("<TemporaryDurationAdjustment>k__BackingField");
  }

  // ─────────────────────────────────────────────
  // 1) Mark context ONLY during Salvation-of-Time AdjustDurationBeneficialEffects call
  // AdjustStatusEffectDurations(EffectType, float, StatusEffect)
  // ─────────────────────────────────────────────
  [HarmonyPatch(typeof(CharacterStats))]
  static class Patch_SoT_Context
  {
    static readonly Type[] Sig = new[]
    {
      typeof(CharacterStats.EffectType),
      typeof(float),
      typeof(StatusEffect)
    };

    [HarmonyPatch("AdjustStatusEffectDurations", argumentTypes: null)]
    [HarmonyPrepare]
    static bool Prepare() => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDurations", Sig) != null;

    [HarmonyPatch("AdjustStatusEffectDurations", argumentTypes: null)]
    [HarmonyTargetMethod]
    static System.Reflection.MethodBase TargetMethod()
      => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDurations", Sig);

    static void Prefix(CharacterStats.EffectType effectType, float DurationAdj, StatusEffect excludedEffect)
    {
      // Salvation of Time signature: Beneficial + excludedEffect.AffectsStat == AdjustDurationBeneficialEffects
      if (effectType == CharacterStats.EffectType.Beneficial &&
          DurationAdj > 0f &&
          excludedEffect != null &&
          excludedEffect.Params.AffectsStat == StatusEffect.ModifiedStat.AdjustDurationBeneficialEffects)
      {
        SoTContext.Depth++;
        if (SoTContext.Touched == null)
          SoTContext.Touched = new HashSet<StatusEffect>();
        SoTContext.Touched.Clear();
      }
    }

    // Ensure we always decrement depth even if something throws
    static Exception Finalizer(Exception __exception)
    {
      if (SoTContext.Depth > 0) SoTContext.Depth--;
      return __exception;
    }
  }

  // ─────────────────────────────────────────────
  // 2) Capture ONLY the effects the engine actually adjusts during SoT
  // AdjustStatusEffectDuration(StatusEffect effect, float DurationAdj, bool skipOverride)
  // ─────────────────────────────────────────────
  [HarmonyPatch(typeof(CharacterStats))]
  static class Patch_Capture_Touched
  {
    static readonly Type[] Sig = new[]
    {
      typeof(StatusEffect),
      typeof(float),
      typeof(bool)
    };

    [HarmonyPatch("AdjustStatusEffectDuration", argumentTypes: null)]
    [HarmonyPrepare]
    static bool Prepare() => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDuration", Sig) != null;

    [HarmonyPatch("AdjustStatusEffectDuration", argumentTypes: null)]
    [HarmonyTargetMethod]
    static System.Reflection.MethodBase TargetMethod()
      => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDuration", Sig);

    static void Prefix(StatusEffect effect, float DurationAdj, bool skipOverride)
    {
      if (!SoTContext.Active) return;
      if (DurationAdj <= 0f) return;
      if (effect == null) return;

      SoTContext.Touched?.Add(effect);
    }
  }

  // ─────────────────────────────────────────────
  // 3) After the SoT call, refresh ONLY touched effects to 300s from "now"
  // (Duration=300, TempAdj=0, elapsed=0, interval=0)
  // ─────────────────────────────────────────────
  [HarmonyPatch(typeof(CharacterStats))]
  static class Patch_SoT_Refill
  {
    static readonly Type[] Sig = new[]
    {
      typeof(CharacterStats.EffectType),
      typeof(float),
      typeof(StatusEffect)
    };

    [HarmonyPatch("AdjustStatusEffectDurations", argumentTypes: null)]
    [HarmonyPrepare]
    static bool Prepare() => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDurations", Sig) != null;

    [HarmonyPatch("AdjustStatusEffectDurations", argumentTypes: null)]
    [HarmonyTargetMethod]
    static System.Reflection.MethodBase TargetMethod()
      => AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDurations", Sig);

    static void Postfix(CharacterStats.EffectType effectType, float DurationAdj, StatusEffect excludedEffect)
    {
      if (effectType != CharacterStats.EffectType.Beneficial) return;
      if (DurationAdj <= 0f) return;

      if (excludedEffect == null ||
          excludedEffect.Params.AffectsStat != StatusEffect.ModifiedStat.AdjustDurationBeneficialEffects)
        return;

      var set = SoTContext.Touched;
      if (set == null || set.Count == 0) return;

      const float TARGET = 3600f;

      foreach (var se in set)
      {
        if (se == null) continue;
        if (se == excludedEffect) continue; // SoT 자체 효과 제외
        if (se.Params == null) continue;

        //Infuse With Vital Essence 정수주입은 적용 안되게
        if (se.Params.AffectsStat == StatusEffect.ModifiedStat.MaxHealth
          || se.Params.AffectsStat == StatusEffect.ModifiedStat.MaxStamina)
          continue;

        if (se.PhraseOrigin != null)
          continue;

        // 엔진이 "연장"한 것만 건드리므로, 패시브/오라 필터를 여기서 억지로 더할 필요 없음
        // 리필(재적용) 처리
        SEPriv.Duration(se) = TARGET;
        SEPriv.TempAdj(se) = 0f;
        SEPriv.TimeActive(se) = 0f;
        SEPriv.IntervalTimer(se) = 0f;
      }
    }
  }
}