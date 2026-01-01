using HarmonyLib;
using UnityEngine;

namespace StackableMod
{
  [HarmonyPatch(typeof(StatusEffect), "Suppresses")]
  public static class Patch_StatusEffect_Suppresses
  {
    static bool Prefix(
      StatusEffect __instance,
      StatusEffect eff,
      bool suppress_if_tied,
      ref bool __result)
    {
      if (!Main.Settings.EnableForAbilities)
        return true;

      if (__instance == null || eff == null)
        return true;

      if (eff.Params.AffectsStat == StatusEffect.ModifiedStat.GenericMarker)
      {
        __result = false;
        return false;
      }

      // 🔑 같은 능력일 때만 억제
      if (IsSameAbilitySameEffectSameDirection(__instance, eff))
      {
        Main.LogParams($"[Suppresses]");
        Main.LogParams($" AbilityOrigin : {__instance.AbilityOrigin}");
        Main.LogParams($" EffectID      : {__instance.EffectID} / {eff.EffectID}");
        Main.LogParams($" Origin        : {__instance.CurrentAppliedValue} / {eff.CurrentAppliedValue}");
        Main.LogParams($" Duration      : {__instance.Duration} / {eff.Duration}");
        Main.LogParams($" TimeLeft      : {__instance.TimeLeft} / {eff.TimeLeft}");

        if (__instance.HasBiggerValueThan(eff))
        {
          __result = true;
          return false;
        }

        if (__instance.CurrentAppliedValue == eff.CurrentAppliedValue)
        {
          if (__instance.TimeLeft > eff.TimeLeft ||
              (__instance.TimeLeft == eff.TimeLeft && suppress_if_tied))
          {
            __result = true;
            return false;
          }
        }
      }

      // 그 외는 엔진 원본 로직 사용
      return true;
    }

    static bool IsSameAbilitySameEffectSameDirection(StatusEffect a, StatusEffect b)
    {
      if (a == null || b == null)
        return false;

      // 1️⃣ 같은 능력
      if (a.AbilityOrigin == null || b.AbilityOrigin == null)
        return false;

      if (a.AbilityOrigin != b.AbilityOrigin)
        return false;

      // 2️⃣ 같은 스탯
      if (a.Params == null || b.Params == null)
        return false;

      if (a.Params.AffectsStat != b.Params.AffectsStat)
        return false;

      // 3️⃣ 남은 시간이 같고 다른 값
      if (a.CurrentAppliedValue != b.CurrentAppliedValue && a.TimeLeft == b.TimeLeft)
        return false;

      return true;
    }
  }
}