using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace CipherFocus
{
  public static class Main
  {
    public static Settings Settings;
    // UMM에서 호출되는 진입점
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("CipherFocus loaded (spell casts are now infinite).");
      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("Cipher Start Focus (%)");
      GUILayout.Label($"Current: {Settings.StartFocusPercent} percent");

      Settings.StartFocusPercent = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.StartFocusPercent,
          25f,     // 25%
          100f   // 100%
        )
      );

      GUILayout.Label($"= {Settings.StartFocusPercent} percent");
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
      Settings.Save(modEntry);
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    // 기본 25% - 100%
    public float StartFocusPercent = 25f;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
