using UnityEngine;

public class ActionExecutionor
{
    public void BattleLog(int[] entityOrder,int entityIndex, RuntimeEntity[] entity)
    {
     
        for (int i = 0; i < entityIndex; i++) 
        {
                Debug.Log("Speed : "+ entity[entityOrder[i]].currentSpeed+ " Type : " + entity[entityOrder[i]].BaseData.Type  + entity[entityOrder[i]].BaseData.Editor_ID + " actions");
        }
    }
}
