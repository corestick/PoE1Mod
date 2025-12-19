using HarmonyLib;
using UnityModManagerNet;

namespace EnablePerEncounterSpells
{
  public static class Main
  {
    // UMM에서 호출되는 진입점
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("EnablePerEncounterSpells loaded (spell casts are now infinite).");
      return true;
    }
  }
}
