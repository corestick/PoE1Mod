using HarmonyLib;

namespace CharacterStatsEditor
{
  [HarmonyPatch(typeof(CharacterStats), "Update")]
  public static class Patch_Update
  {
    static void Prefix(CharacterStats __instance)
    {
      if (!Main.Enabled) return;
      if (!Main.ApplyPending) return;
      if (__instance != Main.Selected) return;

      __instance.BaseMight = Main.Buffer.Might;
      __instance.BaseConstitution = Main.Buffer.Con;
      __instance.BaseDexterity = Main.Buffer.Dex;
      __instance.BasePerception = Main.Buffer.Per;
      __instance.BaseIntellect = Main.Buffer.Int;
      __instance.BaseResolve = Main.Buffer.Res;

      Main.ApplyPending = false;
    }
  }
}
