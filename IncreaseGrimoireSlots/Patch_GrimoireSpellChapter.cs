using HarmonyLib;

namespace IncreaseGrimoireSlots
{
  [HarmonyPatch(typeof(Grimoire.SpellChapter), MethodType.Constructor)]
  public static class Patch_Grimoire_SpellChapter_Ctor
  {
    static void Postfix(Grimoire.SpellChapter __instance)
    {
      // SpellData 기본값을 4칸 → 7칸으로 확장
      if (__instance.SpellData.Length != Main.TargetSlots)
        __instance.SpellData = new GenericSpell[Main.TargetSlots];
    }
  }

  // SpellChapter.IsFull() 확장
  [HarmonyPatch(typeof(Grimoire.SpellChapter), "IsFull")]
  public static class Patch_Grimoire_SpellChapter_IsFull
  {
    static bool Prefix(Grimoire.SpellChapter __instance, ref bool __result)
    {
      int count = 0;
      foreach (var s in __instance.SpellData)
        if (s != null)
          count++;

      __result = (count >= __instance.SpellData.Length);
      return false;
    }
  }
}