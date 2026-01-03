using HarmonyLib;
using UnityEngine;

namespace StrongholdAdventureTurn
{
  [HarmonyPatch(typeof(Stronghold))]
  [HarmonyPatch("ActivateStronghold")]
  public static class Patch_Stronghold_SpawnAdventureInterval
  {
    static void Postfix(Stronghold __instance)
    {
      __instance.SpawnAdventureTurnCount = Main.Settings.SpawnAdventureTurnCount;
    }
  }
}