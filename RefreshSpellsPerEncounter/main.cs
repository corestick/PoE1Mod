using HarmonyLib;
using UnityModManagerNet;

namespace RefreshSpellsPerEncounter
{
  public static class Main
  {
    // UMM에서 호출되는 진입점
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("RefreshSpellsPerEncounter loaded (spell casts are now infinite).");
      return true;
    }
  }
}
