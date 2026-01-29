using HarmonyLib;
using UnityEngine;

namespace AlwaysDetectTraps
{
  public static class Main
  {
    public static void Load()
    {
      var h = new Harmony("AlwaysDetectTraps");
      h.PatchAll();
      Debug.Log("[AlwaysDetectTraps] Loaded - traps detectable without stealth!");
    }
  }
}
