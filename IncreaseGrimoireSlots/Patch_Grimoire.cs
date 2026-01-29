using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IncreaseGrimoireSlots
{
  [HarmonyPatch(typeof(Grimoire), "Start")]
  public static class Patch_Grimoire_Start
  {
    static bool Prefix(Grimoire __instance)
    {
      if (__instance.Spells.Length != 8)
      {
        Grimoire.SpellChapter[] array = new Grimoire.SpellChapter[8];
        __instance.Spells.CopyTo(array, 0);
        __instance.Spells = array;
      }
      for (int i = 0; i < __instance.Spells.Length; i++)
      {
        if (__instance.Spells[i] == null)
        {
          __instance.Spells[i] = new Grimoire.SpellChapter();
        }
        else if (__instance.Spells[i].SpellData.Length != Main.TargetSlots)
        {
          GenericSpell[] array2 = new GenericSpell[Main.TargetSlots];
          for (int j = 0; j < Mathf.Min(array2.Length, __instance.Spells[i].SpellData.Length); j++)
          {
            array2[j] = __instance.Spells[i].SpellData[j];
          }
          __instance.Spells[i].SpellData = array2;
        }
      }

      return false;
    }
  }

  //사용가능 스펠 제한을 제거 -> 주문버튼 4개이상 활성화
  [HarmonyPatch(typeof(Grimoire), "HasSpell")]
  public static class Patch_Grimoire_HasSpell
  {
    static bool Prefix(Grimoire __instance, GenericSpell spell, ref bool __result)
    {
      if (__instance?.Spells == null || spell == null)
      {
        __result = false;
        return false;
      }

      int level = spell.SpellLevel - 1;
      if (level < 0 || level >= __instance.Spells.Length)
      {
        __result = false;
        return false;
      }

      foreach (var s in __instance.Spells[level].SpellData)
      {
        if (s?.DisplayName?.StringID == spell.DisplayName?.StringID)
        {
          __result = true;
          return false;
        }
      }

      __result = false;
      return false;
    }
  }
}