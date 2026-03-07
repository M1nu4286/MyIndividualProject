using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public BattleManager.TotalState currentState { get; private set; }



    public void Initialize(BattleManager.TotalState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(BattleManager.TotalState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }


}
