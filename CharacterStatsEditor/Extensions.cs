using System;
using UnityEngine;

namespace CharacterStatsEditor
{
  public static class Extensions
  {
    public static Guid GetCharacterGuid(this CharacterStats stats)
    {
      // 플레이어인 경우
      Player p = stats.GetComponent<Player>();
      if (p != null)
        return p.SessionID;

      // NPC/동료일 경우
      InstanceID inst = stats.GetComponent<InstanceID>();
      if (inst != null)
        return inst.Guid;

      return Guid.Empty;
    }
  }
}
