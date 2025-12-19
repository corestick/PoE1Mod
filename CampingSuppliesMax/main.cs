using System;
using HarmonyLib;
using UnityModManagerNet; // 또는 UnityModManager; UMM DLL 네임스페이스에 맞추기

namespace CampingSuppliesMax
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      modEntry.Logger.Log("CampingSuppliesMax loaded.");

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      return true;
    }
  }

  [HarmonyPatch(typeof(CampingSupplies))]
  public static class CampingSupplies_StackMaximum_Patch
  {
    [HarmonyPatch("StackMaximum", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref int __result)
    {
      // 예시: 난이도 무시하고 항상 8개
      __result = 99;

      // 난이도별로 바꾸고 싶으면 아래 주석 참고해서 커스터마이즈 가능
      /*
      GameDifficulty difficulty = GameState.Instance.Difficulty;
      switch (difficulty)
      {
          case GameDifficulty.Easy:
              __result = 10;
              break;
          case GameDifficulty.Normal:
              __result = 8;
              break;
          case GameDifficulty.Hard:
          case GameDifficulty.PathOfTheDamned:
              __result = 4;
              break;
          case GameDifficulty.StoryTime:
              __result = 99;
              break;
      }
      */
    }
  }
}
