using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityModManagerNet;

namespace CharacterStatsEditor
{
  public class Settings : UnityModManager.ModSettings
  {
    public Dictionary<Guid, SkillSaveData> Skills = new Dictionary<Guid, SkillSaveData>();

    public override void Save(UnityModManager.ModEntry modEntry)
    {
      string path = GetPath(modEntry);
      string text = JsonConvert.SerializeObject(this, Formatting.Indented);
      File.WriteAllText(path, text);
    }

    public static Settings Load(UnityModManager.ModEntry modEntry)
    {
      string path = GetPath(modEntry);

      if (!File.Exists(path))
        return new Settings();

      return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
    }

    private new static string GetPath(UnityModManager.ModEntry modEntry)
    {
      return Path.Combine(modEntry.Path, "Settings.json");
    }
  }
}
