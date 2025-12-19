using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace GrimoireSlots
{
  [HarmonyPatch(typeof(Grimoire), "Start")]
  public static class Patch_Grimoire_Start
  {
    // Start()의 truncate 로직이 SpellData를 4칸으로 줄여버리므로 원본 스킵 필수
    static bool Prefix() => false;
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