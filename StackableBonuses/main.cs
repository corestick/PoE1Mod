using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace StackableBonuses
{
  public static class Main
  {
    public static Settings Settings;
    public static UnityModManager.ModEntry Mod;
    private static bool EnableLog = false;
    public static bool Load(UnityModManager.ModEntry modEntry)
    {
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      Mod = modEntry;   // 👈 이거 추가

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.Logger.Log("StackableBonuses Loaded Successfully!");
      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      EnableLog = GUILayout.Toggle(EnableLog, "Log 사용");

      Settings.EnableForAbilities =
        GUILayout.Toggle(Settings.EnableForAbilities, "능력(Ability)에 적용");

      Settings.EnableForEquipment =
        GUILayout.Toggle(Settings.EnableForEquipment, "장비(Equipment)에 적용");
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
    public bool EnableForAbilities = true;
    public bool EnableForEquipment = true;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
