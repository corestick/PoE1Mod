using HarmonyLib;
using UnityModManagerNet;

namespace DisableFriendlyFireAOE
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("DisableFriendlyFireAOE Loaded Successfully!");
      return true;
    }
  }

}
