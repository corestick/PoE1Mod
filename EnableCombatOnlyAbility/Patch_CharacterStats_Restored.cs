using HarmonyLib;
using UnityModManagerNet;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EnableCombatOnlyAbility
{
  [HarmonyPatch(typeof(CharacterStats), "Restored")]
  public static class Patch_CharacterStats_Restored
  {
    [HarmonyPostfix]
    public static bool Prefix(CharacterStats __instance)
    {
      var abilities = Traverse
          .Create(__instance)
          .Field<BindingList<GenericAbility>>("m_abilities")
          .Value;

      foreach (var ability in abilities)
      {
        if (ability == null)
          continue;

        ability.CombatOnly = false;
      }

      return true;
    }
  }
}