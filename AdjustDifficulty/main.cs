using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityModManagerNet;
using UnityEngine;

namespace AdjustDifficulty
{
  public static class Main
  {
    public static UnityModManager.ModEntry Mod;
    public static Settings Settings;
    private static bool EnableLog = false;
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
      EnableLog = GUILayout.Toggle(EnableLog, "Log 사용");

      GUILayout.Label("HealthStamina Mult");
      GUILayout.Label($"Current: x{Settings.HealthStaminaMult}");

      float step = 0.5f;
      float value =
        GUILayout.HorizontalSlider(
          Settings.HealthStaminaMult,
          0.5f,     // 최소 0.5배
          50f   // 최대 50배
      );

      Settings.HealthStaminaMult = (float)Math.Round((double)(value / step)) * step;

      GUILayout.Label("Stat Bonus");
      GUILayout.Label($"Current: +{Settings.StatBonus}");

      float step2 = 1.0f;
      float value2 =
        GUILayout.HorizontalSlider(
            Settings.StatBonus,
            1.0f,     // 최소
            100f   // 최대
        );

      Settings.StatBonus = (float)Math.Round((double)(value2 / step2)) * step2;
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
      Settings.Save(modEntry);
    }

    public static void LogParams(string msg)
    {
      if (EnableLog)
      {
        Mod.Logger.Log(msg);
      }
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    // 기본
    public float HealthStaminaMult = 1f;
    public float StatBonus = 15f;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
