using System;
using UnityEngine;

public class StateMachine
{
    public TotalState currentState { get; private set; }

    public void Initialize(TotalState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(TotalState newState)
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
