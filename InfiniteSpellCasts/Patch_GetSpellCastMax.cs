using HarmonyLib;
using UnityEngine;

namespace InfiniteSpellCasts
{
  [HarmonyPatch(typeof(SpellMax), nameof(SpellMax.GetSpellCastMax))]
  public static class Patch_GetSpellCastMax
  {
    static void Postfix(GameObject caster, int spellLevel, ref int __result)
    {
      if (caster == null || spellLevel < 1)
        return;

      var stats = caster.GetComponent<CharacterStats>();
      if (stats == null || !stats.CanCastSpells)
      {
        __result = 0;
        return;
      }

      if (caster.GetComponent<PartyMemberAI>() == null)
        return;

      if (__result > 0)
        __result = int.MaxValue;
    }
  }
}