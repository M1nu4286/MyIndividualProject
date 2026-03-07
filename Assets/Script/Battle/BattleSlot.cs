using UnityEngine;

public struct BattleSlot
{
    SkillData skillData;
    string targetID;

    public BattleSlot(SkillData skillData, RuntimeEntity target)
    {
        this.skillData = skillData;
        this.targetID = target.BaseData.EntityID;
    }
}
