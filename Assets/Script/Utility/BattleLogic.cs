using GameData.Types;
using UnityEngine;

public static class BattleLogic
{
    public static int Roll(RuntimeEntity entity)
    {
        return Random.Range(entity.BaseData.MaxSpeed, entity.BaseData.MinSpeed + 1);
    }

    public static int IndexCreator(EntityDatabase entity, EntityType type)
    {
        int index = 0;
        for (int i = 0; i < entity.Entities.Length; i++)
        {
            if (entity.Entities[i].Type == type)
                index++;
        }
        return index;
    }
}
