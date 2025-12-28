using System;
using System.Collections.Generic;
using HarmonyLib;

namespace GrimoireSlots
{
  [HarmonyPatch(typeof(UIGrimoireInSpellRow), "Init")]
  public static class Patch_UIGrimoireInSpellRow_Init
  {
    static void Postfix(UIGrimoireInSpellRow __instance)
    {
      var field = AccessTools.Field(typeof(UIGrimoireInSpellRow), "m_Spells");
      var list = field.GetValue(__instance) as List<UIGrimoireSpell>;
      if (list == null || __instance.RootSpell?.Icon == null)
        return;

      var rootIconGO = __instance.RootSpell.Icon.gameObject;
      var rootListener = UIEventListener.Get(rootIconGO);

      for (int i = list.Count; i < Main.TargetSlots; i++)
      {
        var cloneGO = NGUITools.AddChild(
            __instance.RootSpell.transform.parent.gameObject,
            __instance.RootSpell.gameObject
        );

        var clone = cloneGO.GetComponent<UIGrimoireSpell>();
        if (clone?.Icon == null)
          continue;

        var listener = UIEventListener.Get(clone.Icon.gameObject);
        listener.onClick += rootListener.onClick;
        listener.onRightClick += rootListener.onRightClick;

        list.Add(clone);
      }

      field.SetValue(__instance, list);

      if (__instance.Grid != null)
      {
        __instance.Grid.maxPerLine = 0;
        __instance.Grid.Reposition();
      }
    }
  }
}
