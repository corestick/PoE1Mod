using System;
using HarmonyLib;
using UnityModManagerNet; // 또는 UnityModManager; UMM DLL 네임스페이스에 맞추기

namespace CampingSupplies99
{
  [HarmonyPatch(typeof(CampingSupplies))]
  public static class Patch_CampingSupplies_StackMaximum
  {
    [HarmonyPatch("StackMaximum", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref int __result)
    {
      // 예시: 난이도 무시하고 항상 99개
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
