using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AdjustBuffDuration
{
  [Serializable]
  public class BuffWhitelistConfig
  {
    public bool enabled = true;
    public string mode = "whitelist"; // whitelist | blacklist
    public List<string> origins = new List<string>();

    public List<string> filterlists = new List<string>();
  }

  public static class BuffWhitelistManager
  {
    public static BuffWhitelistConfig Config;

    public static void Load(UnityModManager.ModEntry mod)
    {
      try
      {
        string path = Path.Combine(mod.Path, "buff_whitelist.json");

        if (!File.Exists(path))
        {
          Config = new BuffWhitelistConfig();

          File.WriteAllText(
            path,
            JsonConvert.SerializeObject(Config, Formatting.Indented)
          );

          mod.Logger.Log("buff_whitelist.json 생성됨");
          return;
        }

        string json = File.ReadAllText(path);
        Config = JsonConvert.DeserializeObject<BuffWhitelistConfig>(json);

        mod.Logger.Log($"Whitelist loaded ({Config.origins.Count} entries)");
      }
      catch (Exception e)
      {
        mod.Logger.Log($"Whitelist load failed: {e}");
        Config = new BuffWhitelistConfig();
      }
    }

    public static bool IsAllowed(string originName)
    {
      if (Config == null || !Config.enabled)
        return true;

      if (string.IsNullOrEmpty(originName))
        return true;

      bool contains = Config.origins.Contains(originName);

      return Config.mode == "whitelist" ? contains : !contains;
    }

    public static bool IsFilterd(string originName)
    {
      if (Config == null || !Config.enabled)
        return true;

      if (string.IsNullOrEmpty(originName))
        return true;

      bool contains = Config.filterlists.Contains(originName);

      return Config.mode == "whitelist" ? contains : !contains;
    }
  }
}