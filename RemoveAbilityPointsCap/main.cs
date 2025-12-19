using HarmonyLib;
using System.Collections.Generic;
using UnityModManagerNet; // 또는 UnityModManager; UMM DLL 네임스페이스에 맞추기

namespace RemoveAbilityPointsCap
{
  public static class Main
  {
    public static void Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();
      modEntry.Logger.Log("Ability Level Loop Patch Loaded");
    }
  }

  [HarmonyPatch(typeof(AbilityProgressionTable))]
  public static class AbilityProgressionTablePatch
  {
    // Patch target: GetLevelAbilityPoints(int level)
    [HarmonyPatch("GetLevelAbilityPoints")]
    [HarmonyPrefix]
    public static bool Prefix(AbilityProgressionTable __instance, ref int level, ref List<AbilityProgressionTable.AbilityPointUnlock> __result)
    {
      // 1) 1~16 범위로 순환시키기
      level = level % 16;
      if (level == 0) level = 16;

      // 2) 기존 static 리스트 사용 대신 안전한 새 리스트 생성
      var list = new List<AbilityProgressionTable.AbilityPointUnlock>();

      foreach (var unlock in __instance.AbilityPointUnlocks)
      {
        if (unlock.Level == level)
        {
          list.Add(unlock);
        }
      }

      // 결과 반환
      __result = list;

      // 원본 함수 실행 막기
      return false;
    }
  }


}
