using UnityEngine;

public static class BattleLogic
{
    public static int Roll(RuntimeEntity entity)
    {
         return Random.Range(entity.BaseData.minSpeed, entity.BaseData.maxSpeed + 1);
    }
}
