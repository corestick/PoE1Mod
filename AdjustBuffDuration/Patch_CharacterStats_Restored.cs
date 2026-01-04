// using HarmonyLib;
// using UnityModManagerNet;
// using UnityEngine;
// using System;
// using System.Reflection;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using System.ComponentModel;
// using System.IO;
// using System.Text;
// using Newtonsoft.Json;

// namespace AdjustBuffDuration
// {
//   [HarmonyPatch(typeof(CharacterStats), "Restored")]
//   public static class Patch_CharacterStats_Restored
//   {
//     static bool Prefix(CharacterStats __instance)
//     {
//       var abilities = Traverse.Create(__instance).Field<BindingList<GenericAbility>>("m_abilities")?.Value;
//       if (abilities == null)
//         return true;

//       foreach (var ability in abilities)
//       {
//         if (ability == null)
//           continue;

//         ability.CombatOnly = false;
//         var effects = Traverse.Create(ability).Field<BindingList<StatusEffect>>("m_effects").Value;

//         if (effects == null)
//           continue;

//         foreach (var se in effects)
//         {
//           if (se == null)
//             continue;

//           Main.LogEffect(se);

//           if (ShouldApply(se))
//           {
//             se.Duration = Main.Settings.BuffDurationMinutes * 60f;
//             se.m_durationOverride = Main.Settings.BuffDurationMinutes * 60f;
//           }
//         }
//       }

//       return true;
//     }

//     private static bool ShouldApply(StatusEffect __instance)
//     {
//       // 시전자
//       var owner = __instance.Owner;
//       if (owner == null)
//         return false;

//       var ownerStats = owner.GetComponent<CharacterStats>();
//       if (ownerStats == null || !ownerStats.IsPartyMember)
//         return false;

//       // 대상
//       // var target = __instance.Target;
//       // if (target == null)
//       //   return false;

//       // var targetStats = target.GetComponent<CharacterStats>();
//       // if (targetStats == null || !targetStats.IsPartyMember)
//       //   return false;

//       // // 대상이 우리 편이 아니면
//       // if (targetStats.HasFactionSwapEffect())
//       //   return false;

//       var origin = __instance.Origin;
//       if (origin == null)
//         return false;

//       string originName = origin.name;

//       if (!BuffWhitelistManager.IsAllowed(originName))
//       {
//         if (!BuffWhitelistManager.IsFilterd(originName) && !string.IsNullOrEmpty(originName))
//         {
//           Main.LogEffect(__instance);
//         }
//         return false;
//       }
//       return true;
//     }
//   }
// }