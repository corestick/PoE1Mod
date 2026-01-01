using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace EnableCombatOnlyAbility
{
  public static class Main
  {
    public static Settings Settings;
    public static UnityModManager.ModEntry Mod;

    private static bool EnableLog = false;

    // UMM에서 호출되는 진입점
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      Mod = modEntry;   // 👈 이거 추가

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("EnableCombatOnlyAbility loaded (spell casts are now infinite).");

      Patch_GameResources_OnLoadedSave.Init();
      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      //EnableLog = GUILayout.Toggle(EnableLog, "Log 사용");
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
    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
