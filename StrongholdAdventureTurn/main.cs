using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace StrongholdAdventureTurn
{
  public static class Main
  {
    public static Settings Settings;

    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("StrongholdAdventureTurn Loaded Successfully!");
      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("Spawn Adventure Turn Count");
      GUILayout.Label($"Current: {Settings.SpawnAdventureTurnCount} Turn");

      Settings.SpawnAdventureTurnCount = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.SpawnAdventureTurnCount,
          1,  // 1턴
          20   // 20턴
        )
      );
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
      Settings.Save(modEntry);
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    public int SpawnAdventureTurnCount = 5;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
