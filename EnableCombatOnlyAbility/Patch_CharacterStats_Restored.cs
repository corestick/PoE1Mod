using HarmonyLib;
using UnityModManagerNet;
using System.ComponentModel;

namespace EnableCombatOnlyAbility
{
  [HarmonyPatch(typeof(CharacterStats), "Restored")]
  public static class CharacterStats_Restored_Postfix
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