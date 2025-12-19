using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityModManagerNet;

namespace AdjustDurationBeneficial
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      try
      {
        var harmony = new Harmony(modEntry.Info.Id);
        harmony.PatchAll();
        modEntry.Logger.Log("AdjustDurationBeneficialEffects loaded");
        return true;
      }
      catch (Exception e)
      {
        modEntry.Logger.Error(e.ToString());
        return false;
      }
    }
  }
}
