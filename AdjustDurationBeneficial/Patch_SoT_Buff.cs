// using HarmonyLib;
// using System;
// using UnityModManagerNet;

// namespace AdjustDurationBeneficial
// {
//   // ─────────────────────────────────────────────
//   // SoT context flag
//   // ─────────────────────────────────────────────
//   static class SoTContext
//   {
//     [ThreadStatic] public static bool Active;
//   }

//   // ─────────────────────────────────────────────
//   // Mark when Salvation of Time is running
//   // AdjustStatusEffectDurations(EffectType, float, StatusEffect)
//   // ─────────────────────────────────────────────
//   [HarmonyPatch(typeof(CharacterStats))]
//   static class Patch_SoT_Context
//   {
//     static readonly Type[] Sig = new[]
//     {
//       typeof(CharacterStats.EffectType),
//       typeof(float),
//       typeof(StatusEffect)
//     };

//     static System.Reflection.MethodBase TargetMethod()
//     {
//       return AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDurations", Sig);
//     }

//     [HarmonyTargetMethod]
//     static System.Reflection.MethodBase Target() => TargetMethod();

//     static void Prefix(
//       CharacterStats.EffectType effectType,
//       float DurationAdj,
//       StatusEffect excludedEffect
//     )
//     {
//       if (effectType == CharacterStats.EffectType.Beneficial &&
//           DurationAdj > 0f &&
//           excludedEffect != null &&
//           excludedEffect.Params.AffectsStat ==
//             StatusEffect.ModifiedStat.AdjustDurationBeneficialEffects)
//       {
//         SoTContext.Active = true;
//       }
//     }

//     static void Postfix()
//     {
//       SoTContext.Active = false;
//     }
//   }

//   // ─────────────────────────────────────────────
//   // Multiply duration adjustment
//   // AdjustStatusEffectDuration(StatusEffect, float, bool)
//   // ─────────────────────────────────────────────
//   [HarmonyPatch(typeof(CharacterStats))]
//   static class Patch_MultiplyDurationAdj
//   {
//     static readonly Type[] Sig = new[]
//     {
//       typeof(StatusEffect),
//       typeof(float),
//       typeof(bool)
//     };

//     static System.Reflection.MethodBase TargetMethod()
//     {
//       return AccessTools.Method(typeof(CharacterStats), "AdjustStatusEffectDuration", Sig);
//     }

//     [HarmonyTargetMethod]
//     static System.Reflection.MethodBase Target() => TargetMethod();

//     static void Prefix(
//       ref float DurationAdj
//     )
//     {
//       if (!SoTContext.Active)
//         return;

//       if (effect == null || effect.Params == null)
//         return;

//       // 🔒 MaxHealth 관련 효과는 증폭하지 않음
//       if (effect.Params.AffectsStat == StatusEffect.ModifiedStat.MaxHealth)
//         return;

//       // 원래 +10 → +100
//       DurationAdj *= 10f;
//     }
//   }
// }
