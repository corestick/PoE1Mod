using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IncreaseGrimoireSlots
{
  public static class Main
  {
    public const int TargetSlots = 7;
    public static UnityModManager.ModEntry Mod;
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      Mod = modEntry;   // 👈 이거 추가

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("IncreaseGrimoireSlots Loaded Successfully!");
      return true;
    }
  }

}
