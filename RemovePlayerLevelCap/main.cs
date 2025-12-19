using HarmonyLib;
using System.Collections.Generic;
using UnityModManagerNet; // 또는 UnityModManager; UMM DLL 네임스페이스에 맞추기

namespace RemovePlayerLevelCap
{
  public static class Main
  {
    public static void Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("Player Level Cap Patch Loaded (max 99)");
    }
  }

  [HarmonyPatch(typeof(CharacterStats), "get_PlayerLevelCap")]
  public static class PlayerLevelCapPatch
  {
    [HarmonyPostfix]
    public static void Postfix(ref int __result)
    {
      __result = 99;
    }
  }
}
