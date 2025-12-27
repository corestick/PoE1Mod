using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace AdjustMovementSpeed
{
  [HarmonyPatch(typeof(PartyMemberAI))]
  [HarmonyPatch("Update")]
  public static class Patch_PartyMemberAI_Upate
  {
    private static void Postfix(PartyMemberAI __instance)
    {
      var mover = Traverse.Create(__instance)
                          .Field<Mover>("m_mover")
                          .Value;

      if (mover == null)
        return;

      // 🕵️ 은신 상태
      if (Stealth.IsInStealthMode(__instance.gameObject))
      {
        mover.UseCustomSpeed(Main.Settings.StealthMoveSpeed);
      }
      // 🚶 기본 이동
      else if (!GameState.InCombat)
      {
        mover.UseCustomSpeed(Main.Settings.BaseMoveSpeed);
      }
    }
  }
}