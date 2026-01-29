using HarmonyLib;
using UnityEngine;

namespace AlwaysDetectTraps
{
  [HarmonyPatch(typeof(CharacterStats), "DetectionRange")]
  public static class Patch_CharacterStats_DetectionRange
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
