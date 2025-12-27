using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityModManagerNet;
using UnityEngine;

namespace AdjustMovementSpeed
{
  public static class Main
  {
    public static Settings Settings;

    static bool Load(UnityModManager.ModEntry modEntry)
    {
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("Movement Speed Settings");

      // 🔹 기본 이동 속도
      GUILayout.Space(10);
      GUILayout.Label($"Base Move Speed: {Settings.BaseMoveSpeed}");

      Settings.BaseMoveSpeed = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.BaseMoveSpeed,
          6,    // 최소
          20    // 최대
        )
      );

      // 🔹 은신 이동 속도
      GUILayout.Space(10);
      GUILayout.Label($"Stealth Move Speed: {Settings.StealthMoveSpeed}");

      Settings.StealthMoveSpeed = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.StealthMoveSpeed,
          2,    // 최소
          20    // 최대
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
    // 기본 이동 속도
    public int BaseMoveSpeed = 6;

    // 은신 이동 속도
    public int StealthMoveSpeed = 4;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}

