using UnityEngine;

public struct RuntimeEntity
{
    public readonly EntityData BaseData;
    public int currentHp;
    public int currentMental;
    public int currentSpeed;
    public bool isAlive;
    public int index;
        //=> currentHp > 0;
    public RuntimeEntity(EntityData entityData, int index)
        {
            this.BaseData = entityData;
            this.currentHp = entityData.maxHp;
            this.currentMental = 0;
            this.currentSpeed = 0;
            this.isAlive = true;
            this.index = index;
    }
}
