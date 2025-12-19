using HarmonyLib;
using UnityModManagerNet;

namespace ItemStackable
{
  public static class Main
  {
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("ItemStackable Loaded Successfully!");
      return true;
    }
  }
}
