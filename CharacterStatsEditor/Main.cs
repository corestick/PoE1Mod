using System;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace CharacterStatsEditor
{
  public static class Main
  {
    internal static UnityModManager.ModEntry Mod;
    internal static Harmony Harmony;
    internal static UnityModManager.ModEntry.ModLogger Logger;

    internal static bool Enabled = false;
    internal static bool ApplyPending = false;

    internal static CharacterStats Selected;
    internal static CharacterStats LastSelected;

    internal static StatsBuffer Buffer = new StatsBuffer();

    internal static Settings Settings;

    public static void Load(UnityModManager.ModEntry modEntry)
    {
      Mod = modEntry;
      Logger = modEntry.Logger;

      Settings = Settings.Load(modEntry); // ← JSON 로드

      modEntry.OnToggle = OnToggle;
      modEntry.OnUpdate = OnUpdate;
      modEntry.OnGUI = OnGUI;

      Harmony = new Harmony(modEntry.Info.Id);
    }

    static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
    {
      Enabled = value;

      if (value)
      {
        Harmony.PatchAll();
        Logger.Log("CharacterStatsEditor enabled.");
      }
      else
      {
        Harmony.UnpatchAll(modEntry.Info.Id);
        Logger.Log("CharacterStatsEditor disabled.");
      }

      return true;
    }

    static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
    {
      if (!Enabled) return;

      Selected = UIGlobalSelectAPartyMember.Instance.SelectedCharacter;
      if (Selected != null && Selected != LastSelected)
      {
        LoadStatsIntoBuffer(Selected);
        ApplyPending = false;
        LastSelected = Selected;
      }
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
      GUILayout.Label("<b><size=18>Character Stats Editor</size></b>", Rich());

      if (Selected == null)
      {
        GUILayout.Label("선택된 캐릭터 없음");
        return;
      }

      GUILayout.Label($"<b>Character: {Selected.Name()}</b>", Rich());
      GUILayout.Space(10);

      // ---- Attributes ----
      GUILayout.Label("<b>Attributes</b>", Rich());
      DrawStat("Might", ref Buffer.Might, Selected.BaseMight);
      DrawStat("Constitution", ref Buffer.Con, Selected.BaseConstitution);
      DrawStat("Dexterity", ref Buffer.Dex, Selected.BaseDexterity);
      DrawStat("Perception", ref Buffer.Per, Selected.BasePerception);
      DrawStat("Intellect", ref Buffer.Int, Selected.BaseIntellect);
      DrawStat("Resolve", ref Buffer.Res, Selected.BaseResolve);

      GUILayout.Space(12);

      // ---- Skills ----
      GUILayout.Label("<b>Skills</b>", Rich());

      Guid id = Selected.GetCharacterGuid();

      if (!Settings.Skills.TryGetValue(id, out var saved))
        saved = new SkillSaveData();

      DrawStat("Stealth", ref Buffer.Stealth, saved.Stealth);
      DrawStat("Athletics", ref Buffer.Athletics, saved.Athletics);
      DrawStat("Lore", ref Buffer.Lore, saved.Lore);
      DrawStat("Mechanics", ref Buffer.Mechanics, saved.Mechanics);
      DrawStat("Survival", ref Buffer.Survival, saved.Survival);

      GUILayout.Space(15);

      bool anyChanged = HasAnyChanges();

      var applyStyle = new GUIStyle(GUI.skin.button);
      applyStyle.richText = true;
      applyStyle.normal.textColor = anyChanged ? Color.white : Color.gray;

      if (GUILayout.Button("<b>Apply Changes</b>", applyStyle, GUILayout.Width(200), GUILayout.Height(28)))
      {
        if (anyChanged)
        {
          SaveSkills(Selected);
          Settings.Save(Mod);     // JSON 즉시 저장
          ApplyPending = true;    // Attributes 적용 예약
        }
      }
    }

    internal static void LoadStatsIntoBuffer(CharacterStats c)
    {
      Buffer.Might = c.BaseMight;
      Buffer.Con = c.BaseConstitution;
      Buffer.Dex = c.BaseDexterity;
      Buffer.Per = c.BasePerception;
      Buffer.Int = c.BaseIntellect;
      Buffer.Res = c.BaseResolve;

      Guid id = c.GetCharacterGuid();
      if (Settings.Skills.TryGetValue(id, out var s))
      {
        Buffer.Stealth = s.Stealth;
        Buffer.Athletics = s.Athletics;
        Buffer.Lore = s.Lore;
        Buffer.Mechanics = s.Mechanics;
        Buffer.Survival = s.Survival;
      }
      else
      {
        Buffer.Stealth = c.StealthBonus;
        Buffer.Athletics = c.AthleticsBonus;
        Buffer.Lore = c.LoreBonus;
        Buffer.Mechanics = c.MechanicsBonus;
        Buffer.Survival = c.SurvivalBonus;
      }
    }

    internal static void SaveSkills(CharacterStats c)
    {
      Guid id = c.GetCharacterGuid();

      SkillSaveData s = new SkillSaveData
      {
        Stealth = Buffer.Stealth,
        Athletics = Buffer.Athletics,
        Lore = Buffer.Lore,
        Mechanics = Buffer.Mechanics,
        Survival = Buffer.Survival
      };

      Settings.Skills[id] = s;
    }

    internal static bool HasAnyChanges()
    {
      if (Selected == null) return false;

      Guid id = Selected.GetCharacterGuid();
      SkillSaveData saved;
      Settings.Skills.TryGetValue(id, out saved);

      if (saved == null)
        saved = new SkillSaveData();

      // Attributes
      if (Buffer.Might != Selected.BaseMight) return true;
      if (Buffer.Con != Selected.BaseConstitution) return true;
      if (Buffer.Dex != Selected.BaseDexterity) return true;
      if (Buffer.Per != Selected.BasePerception) return true;
      if (Buffer.Int != Selected.BaseIntellect) return true;
      if (Buffer.Res != Selected.BaseResolve) return true;

      // Skills (기준: 저장된 값, 없으면 0)
      if (Buffer.Stealth != saved.Stealth) return true;
      if (Buffer.Athletics != saved.Athletics) return true;
      if (Buffer.Lore != saved.Lore) return true;
      if (Buffer.Mechanics != saved.Mechanics) return true;
      if (Buffer.Survival != saved.Survival) return true;

      return false;
    }

    internal static void DrawStat(string label, ref int buffer, int original)
    {
      GUILayout.BeginHorizontal();

      bool changed = buffer != original;

      GUILayout.Label($"{(changed ? "* " : "  ")}{label}: {buffer}", Rich(), GUILayout.Width(200));
      if (changed)
        GUILayout.Label($"(was {original})", GUILayout.Width(80));
      else
        GUILayout.Space(80);

      if (GUILayout.Button("<", GUILayout.Width(30))) buffer--;
      if (GUILayout.Button(">", GUILayout.Width(30))) buffer++;

      GUILayout.EndHorizontal();
    }

    internal static GUIStyle Rich()
    {
      return new GUIStyle(GUI.skin.label) { richText = true };
    }
  }

  public class StatsBuffer
  {
    public int Might, Con, Dex, Per, Int, Res;
    public int Stealth, Athletics, Lore, Mechanics, Survival;
  }
}
