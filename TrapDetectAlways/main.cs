using HarmonyLib;
using UnityEngine;

namespace TrapDetectAlways
{
  public static class Main
  {
    public static void Load()
    {
      var h = new Harmony("TrapDetectAlwaysMod");
      h.PatchAll();
      Debug.Log("[TrapDetectAlways] Loaded - traps detectable without stealth!");
    }
  }

  [HarmonyPatch(typeof(CharacterStats), "DetectionRange")]
  public static class TrapDetectionPatch
  {
    static bool Prefix(ref float __result, CharacterStats __instance, Detectable d)
    {
      int num = __instance.CalculateSkill(CharacterStats.SkillType.Mechanics);

      // remove stealth penalty
      num++; // original +1

      if (d != null)
        num -= d.GetDifficulty();

      __result = num;
      return false;
    }
  }

}
