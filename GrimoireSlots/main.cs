using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace GrimoireSlots
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("GrimoireSlots Loaded Successfully!");
      return true;
    }
  }

}
