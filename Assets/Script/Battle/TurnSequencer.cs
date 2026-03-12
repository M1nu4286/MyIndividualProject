using GameData.Types;
using UnityEngine;

public class TurnSequencer : MonoBehaviour
{
    private SkillSlotManager _slotManager;
    private SkillDatabase _skillDb;
    private EntityDatabase _entityDb;

    public void InitHash(EntityDatabase entityDatabase, SkillDatabase skillDatabase, SkillSlotManager skillSlotManager)
    {
        _entityDb = entityDatabase;
        _skillDb = skillDatabase;
        _slotManager = skillSlotManager;
    }

    public void ProcessTurn(int[] sortedPlayerIndices, RuntimeEntity[] entityDatas)
    {
        RuntimeEntity[] actors = new RuntimeEntity[sortedPlayerIndices.Length];
        for (int i = 0; i < sortedPlayerIndices.Length; i++)
        {
            actors[i] = entityDatas[sortedPlayerIndices[i]];
        }

        _slotManager.InitializeBattleUI(actors, _skillDb);
    }
}