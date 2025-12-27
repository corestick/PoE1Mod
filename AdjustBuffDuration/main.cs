using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityModManagerNet;
using UnityEngine;

namespace AdjustBuffDuration
{
  public static class Main
  {
    public static UnityModManager.ModEntry Mod;
    public static Settings Settings;

    static bool Load(UnityModManager.ModEntry modEntry)
    {
      Mod = modEntry;   // 👈 이거 추가
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("Salvation of Time – Buff Duration");

      GUILayout.Label($"Current: {Settings.SoTDurationMinutes} minutes");

      Settings.SoTDurationMinutes = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.SoTDurationMinutes,
          1f,     // 1분
          1000f   // 1000분 = 60000초
        )
      );

      GUILayout.Label($"= {Settings.SoTDurationMinutes * 60} seconds");
    }


    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
      Settings.Save(modEntry);
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    // 기본 10분 = 600초
    public int SoTDurationMinutes = 10;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
