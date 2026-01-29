using System;
using System.Collections.Generic;
using HarmonyLib;

namespace EnableCombatOnlyAbilities
{
  [HarmonyPatch(typeof(StatusEffect), "get_AppliedFXNames")]
  public static class Patch_StatusEffect_get_AppliedFXNames
  {
    [HarmonyPrefix]
    public static bool Prefix(StatusEffect __instance, ref string[] __result)
    {
      try
      {
        // 원래 FX가 있으면 원본 로직 사용
        var fx = Traverse.Create(__instance)
          .Field<List<UnityEngine.Object>>("m_appliedFX") // 필드명은 실제 덤프에 맞춰 조정
          .Value;

        if (fx == null)
        {
          __result = Array.Empty<string>();
          return false;
        }

        // FX 중 null 제거
        var names = new List<string>();
        foreach (var f in fx)
        {
          if (f != null)
            names.Add(f.name);
        }

        __result = names.ToArray();
        return false;
      }
      catch
      {
        // 어떤 상황에서도 저장을 깨지 않게
        __result = Array.Empty<string>();
        return false;
      }
    }
  }
}