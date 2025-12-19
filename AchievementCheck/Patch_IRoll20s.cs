using HarmonyLib;
using UnityEngine;

namespace AchievementCheck
{
  // CommandLine.IRoll20s()를 가로채서
  // "치트 토글"만 수행하고 업적 비활성화 로직을 제거한다.
  [HarmonyPatch(typeof(CommandLine), "IRoll20s")]
  internal static class CommandLine_IRoll20s_Patch
  {
    private static bool Prefix()
    {
      // 1️⃣ 치트 토글
      GameState.Instance.CheatsEnabled = !GameState.Instance.CheatsEnabled;

      // 2️⃣ 치트가 켜졌을 때만 업적 상태 점검
      if (GameState.Instance.CheatsEnabled)
      {
        global::Console.AddMessage("Cheats Enabled");

        if (AchievementTracker.Instance == null)
        {
          global::Console.AddMessage(
              "Achievements: tracker not initialized",
              Color.yellow
          );
        }
        else if (AchievementTracker.Instance.DisableAchievements)
        {
          global::Console.AddMessage(
              "Achievements: DISABLED for this save",
              Color.red
          );
        }
        else
        {
          global::Console.AddMessage(
              "Achievements: ENABLED",
              Color.green
          );
        }
      }
      else
      {
        global::Console.AddMessage("Cheats Disabled");
      }

      // 3️⃣ 원본 IRoll20s 차단 (업적 비활성화 로직 포함)
      return false;
    }
  }
}
