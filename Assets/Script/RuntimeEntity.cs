using UnityEngine;

public struct RuntimeEntity
{
    public readonly EntityDatabase BaseData;
    public int currentHp;
    public int currentMental;
    public int currentSpeed;
    public bool isAlive;
    public int index;
        //=> currentHp > 0;
    public RuntimeEntity(EntityDatabase entityData, int index)
        {
            this.BaseData = entityData;
            this.currentHp = entityData.MaxHp;
            this.currentMental = 0;
            this.currentSpeed = 0;
            this.isAlive = true;
            this.index = index;
    }
}
