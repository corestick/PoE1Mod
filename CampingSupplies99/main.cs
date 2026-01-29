using System;
using HarmonyLib;
using UnityModManagerNet; // 또는 UnityModManager; UMM DLL 네임스페이스에 맞추기

namespace CampingSupplies99
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      modEntry.Logger.Log("CampingSupplies99 loaded.");

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      return true;
    }
  }
}
