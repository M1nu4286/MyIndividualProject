using UnityEngine;

public class TurnSequencer 
{


    public void TurnStart(RuntimeEntity[] entities)
    {
        Debug.Log("Turn Start!");
        
    }

    public void TurnEnd() 
    {
        Debug.Log("Idle -> Evaluation");
    }
}
