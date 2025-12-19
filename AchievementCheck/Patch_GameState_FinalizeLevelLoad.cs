using HarmonyLib;
using UnityEngine;

namespace AchievementCheck
{
  // [HarmonyPatch(typeof(GameState), "FinalizeLevelLoad")]
  // public static class Patch_GameState_FinalizeLevelLoad_AchievementCheck
  // {
  //   static void Postfix()
  //   {
  //     if (AchievementTracker.Instance != null)
  //     {
  //       if (AchievementTracker.Instance.DisableAchievements)
  //       {
  //         global::Console.AddMessage(
  //             "⚠ This save has achievements DISABLED",
  //             Color.red
  //         );
  //       }
  //       else
  //       {
  //         global::Console.AddMessage(
  //             "This save has achievements ENABLED",
  //             Color.green
  //         );
  //       }
  //     }
  //   }
  // }
}