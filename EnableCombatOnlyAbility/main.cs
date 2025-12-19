using HarmonyLib;
using UnityModManagerNet;

namespace EnableCombatOnlyAbility
{
  public static class Main
  {
    // UMM에서 호출되는 진입점
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("EnableCombatOnlyAbility loaded (spell casts are now infinite).");
      return true;
    }
  }
}
