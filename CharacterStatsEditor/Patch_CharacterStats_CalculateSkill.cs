using System;
using HarmonyLib;

namespace CharacterStatsEditor
{
  [HarmonyPatch(typeof(CharacterStats), "CalculateSkill")]
  public static class Patch_CalculateSkill
  {
    static void Postfix(CharacterStats __instance,
        CharacterStats.SkillType skillType, ref int __result)
    {
      if (!Main.Enabled) return;

      Guid id = __instance.GetCharacterGuid();
      if (!Main.Settings.Skills.TryGetValue(id, out var s))
        return;

      switch (skillType)
      {
        case CharacterStats.SkillType.Stealth:
          __result += s.Stealth; break;
        case CharacterStats.SkillType.Athletics:
          __result += s.Athletics; break;
        case CharacterStats.SkillType.Lore:
          __result += s.Lore; break;
        case CharacterStats.SkillType.Mechanics:
          __result += s.Mechanics; break;
        case CharacterStats.SkillType.Survival:
          __result += s.Survival; break;
      }
    }
  }
}
