using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace DoubleTalentPoints
{
  [HarmonyPatch(typeof(AbilityProgressionTable), "GetLevelAbilityPoints")]
  class Patch_DoubleTalentPoints
  {
    static void Postfix(ref List<AbilityProgressionTable.AbilityPointUnlock> __result)
    {
      if (__result == null) return;

      if (Main.Settings == null)
        return;

      int value = Main.Settings.TalentPointsPerLevel;

      foreach (var unlock in __result)
      {
        foreach (var pair in unlock.CategoryPointPairs)
        {
          if (pair.Category == AbilityProgressionTable.CategoryFlag.Talent)
          {
            pair.PointsGranted = value; // 고정 2
          }

          if (pair.Category == AbilityProgressionTable.CategoryFlag.Custom1
            || pair.Category == AbilityProgressionTable.CategoryFlag.Custom2
            || pair.Category == AbilityProgressionTable.CategoryFlag.Custom3
            || pair.Category == AbilityProgressionTable.CategoryFlag.Custom4
            || pair.Category == AbilityProgressionTable.CategoryFlag.Custom5
          )
          {
            if (pair.PointsGranted < value)
              pair.PointsGranted = value; // 고정 2
          }
        }
      }
    }
  }

}