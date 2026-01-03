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
      if (!Main.Settings.EnableForAbilities
          && __instance.AbilityType != GenericAbility.AbilityType.Ability)
        return true;

      __result = false;

      if (eff.Params.AffectsStat == StatusEffect.ModifiedStat.GenericMarker)
      {
        return false;
      }

      if (__instance == null || eff == null)
      {
        return false;
      }

      if (__instance.AfflictionOrigin != null && eff.AfflictionOrigin != null
        && __instance.AfflictionOrigin.OverridesAffliction(eff.AfflictionOrigin))
      {
        __result = true;
        return false;
      }

      if (__instance.Origin == null
        || eff.Origin == null
        || __instance.Params == null
        || eff.Params == null)
      {
        return false;
      }

      // 🔑 같은 능력일 때만 억제
      if (IsSameEffect(__instance, eff))
      {
        __result = true;
      }

      // 엔진 원본 로직 스킵
      return false;
    }

    static bool IsSameEffect(StatusEffect a, StatusEffect b)
    {
      // 1️⃣ 같은 능력
      if (a.Origin != b.Origin)
        return false;

      // 2️⃣ 같은 스탯
      if (a.Params == null || b.Params == null)
        return false;

      // 효과
      if (a.Params.AffectsStat != b.Params.AffectsStat)
        return false;

      // 시간이 같으면
      if (a.TimeLeft > 0f && a.TimeLeft == b.TimeLeft)
        return false;

      // 같은 값일 경우 시간이 작은 쪽 억제
      if (a.CurrentAppliedValue == b.CurrentAppliedValue
        && a.TimeLeft < b.TimeLeft)
        return false;

      return true;
    }

    private static void TestLog(StatusEffect __instance, StatusEffect eff)
    {
      Main.LogParams($" << Suppresses >>");
      Main.LogParams($" Origin                : {__instance.Origin.name} / {eff.Origin.name}");
      Main.LogParams($" AffectsStat           : {__instance.Params.AffectsStat} / {eff.Params.AffectsStat}");
      Main.LogParams($" EffectID              : {__instance.EffectID} / {eff.EffectID}");
      Main.LogParams($" CurrentAppliedValue   : {__instance.CurrentAppliedValue} / {eff.CurrentAppliedValue}");
      Main.LogParams($" TimeLeft              : {__instance.TimeLeft} / {eff.TimeLeft}");
      Main.LogParams($" AbilityType           : {__instance.AbilityType} / {eff.AbilityType}");

      Main.LogParams($" BundleName            : {__instance.BundleName} / {eff.BundleName}");
      Main.LogParams($" GetStackingKey        : {__instance.GetStackingKey()} / {eff.GetStackingKey()}");
      Main.LogParams($" NonstackingEffectType : {__instance.NonstackingEffectType} / {eff.NonstackingEffectType}");
    }
  }
}