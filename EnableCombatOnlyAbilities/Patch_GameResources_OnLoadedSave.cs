using HarmonyLib;
using UnityModManagerNet;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace EnableCombatOnlyAbilities
{
  public static class Patch_GameResources_OnLoadedSave
  {
    // 로드 세션 중인지 여부
    private static bool pendingCleanup = false;

    // 중복 처리 방지용
    private static HashSet<int> processedStats = new HashSet<int>();

    public static void Init()
    {
      // 중복 등록 방지
      GameResources.OnLoadedSave -= OnLoadedSave;
      GameResources.OnLoadedSave += OnLoadedSave;

      CharacterStats.s_OnCharacterStatsStart -= OnCharacterStatsStart;
      CharacterStats.s_OnCharacterStatsStart += OnCharacterStatsStart;
    }

    /// <summary>
    /// 세이브 파일이 메모리에 완전히 로드된 시점
    /// </summary>
    private static void OnLoadedSave()
    {
      pendingCleanup = true;
      processedStats.Clear();
    }

    /// <summary>
    /// CharacterStats 하나가 완전히 초기화된 시점
    /// (StatusEffect / 장비 / 패시브 모두 적용 완료)
    /// </summary>
    private static void OnCharacterStatsStart(object sender, EventArgs e)
    {
      if (!pendingCleanup)
        return;

      CharacterStats stats = sender as CharacterStats;
      if (stats == null)
        return;

      if (!IsCurrentPartyMember(stats))
        return;

      int id = stats.GetInstanceID();
      if (processedStats.Contains(id))
        return;

      processedStats.Add(id);
      CleanupEffects(stats);
    }

    /// <summary>
    /// 실제 제거 로직
    /// </summary>
    private static void CleanupEffects(CharacterStats stats)
    {
      if (GameState.InCombat)
        return;

      IList<StatusEffect> effects = stats.ActiveStatusEffects;
      if (effects == null)
        return;

      for (int i = effects.Count - 1; i >= 0; i--)
      {
        StatusEffect se = effects[i];

        if (se == null)
          continue;

        if (se.TimeLeft > 0f)
        {
          if (se.AbilityType == GenericAbility.AbilityType.Ability || se.AbilityType == GenericAbility.AbilityType.Talent)
            stats.ClearEffect(se);
        }
      }
    }

    private static bool IsCurrentPartyMember(CharacterStats stats)
    {
      if (!stats)
        return false;

      // 파티원만
      if (!stats.GetComponent<PartyMemberAI>())
        return false;

      // 적 / 몬스터 제외
      if (stats.GetComponent<BestiaryReference>())
        return false;

      // 고정 오브젝트 제외
      Persistence p = stats.GetComponent<Persistence>();
      if (p != null && !p.Mobile)
        return false;

      return true;
    }
  }
}