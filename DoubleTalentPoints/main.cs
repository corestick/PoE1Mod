using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityModManagerNet;
using UnityEngine;

namespace DoubleTalentPoints
{
  public static class Main
  {
    public static Settings Settings;
    public static UnityModManager.ModEntry ModEntry;

    static bool Load(UnityModManager.ModEntry modEntry)
    {
      ModEntry = modEntry;
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("DoubleTalentPoints loaded (spell casts are now infinite).");
      return true;
    }

    private static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("레벨업 시 재능 포인트");

      Settings.TalentPointsPerLevel =
          (int)GUILayout.HorizontalSlider(
              Settings.TalentPointsPerLevel,
              1,
              10);

      GUILayout.Label($"현재 값: {Settings.TalentPointsPerLevel}");
    }

    private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
      Settings.Save(modEntry);
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    public int TalentPointsPerLevel = 2;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
