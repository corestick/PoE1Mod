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
    private static bool EnableLog = false;
    static bool Load(UnityModManager.ModEntry modEntry)
    {
      BuffWhitelistManager.Load(modEntry); // 버프 적용 목록

      Mod = modEntry;   // 👈 이거 추가
      Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

      var harmony = new Harmony(modEntry.Info.Id);
      harmony.PatchAll();

      modEntry.OnGUI = OnGUI;
      modEntry.OnSaveGUI = OnSaveGUI;

      modEntry.Logger.Log("AdjustBuffDuration Loaded Successfully!");

      // Patch_GameResources_OnLoadedSave.Init();
      return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      EnableLog = GUILayout.Toggle(EnableLog, "Log 사용");

      GUILayout.Label("Buff Duration");
      GUILayout.Label($"Current: {Settings.BuffDurationMinutes} minutes");

      Settings.BuffDurationMinutes = Mathf.RoundToInt(
        GUILayout.HorizontalSlider(
          Settings.BuffDurationMinutes,
          1f,     // 1분
          1000f   // 1000분 = 60000초
        )
      );

      GUILayout.Label($"= {Settings.BuffDurationMinutes * 60} seconds");
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

    public static void LogEffect(StatusEffect __instance)
    {
      LogParams($"[StatusEffect]");
      LogParams($"OriginName : {__instance.Origin.name}");
      LogParams($"BundleName : {__instance.BundleName}");
      LogParams($"AbilityOrigin.name : {__instance.AbilityOrigin.name}");
      LogParams($"AbilityType : {__instance.AbilityType}");
      LogParams($"EffectID : {__instance.EffectID}");
      LogParams($"Owner : {__instance.Owner}");
      LogParams($"Target : {__instance.Target}");

      LogParams($"AffectsStat : {__instance.Params.AffectsStat}");
      LogParams($"param.Duration : {__instance.Params.Duration}");
      LogParams($"TimeLeft : {__instance.TimeLeft}");
      LogParams($"m_durationOverride : {__instance.m_durationOverride}");
      LogParams($"m_needsDurationCalculated : {__instance.m_needsDurationCalculated}");
      LogParams($"CurrentAppliedValue : {__instance.CurrentAppliedValue}");
      LogParams($"IsHostile : {__instance.Params.IsHostile}");
      LogParams($"TemporaryDurationAdjustment : {__instance.TemporaryDurationAdjustment}");
    }
  }

  public class Settings : UnityModManager.ModSettings
  {
    // 기본 10분 = 600초
    public int BuffDurationMinutes = 10;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      Save(this, modEntry);
    }
  }
}
