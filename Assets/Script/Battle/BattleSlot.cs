using GameData.Types;
using UnityEngine;

public struct BattleSlot
{
    SkillEntry skillData;
    int targetID;

    public BattleSlot(SkillEntry skillData, RuntimeEntity target)
    {
        this.skillData = skillData;
        this.targetID = target.BaseData.EntityIDHash;
    }
}
