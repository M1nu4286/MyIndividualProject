using UnityEngine;

public abstract class TotalState
{
    protected StateMachine stateMachine;
    protected BattleManager battleManager;

    public TotalState(StateMachine stateMachine, BattleManager battleManager)
    {
        this.stateMachine = stateMachine;
        this.battleManager = battleManager;
    }

    public virtual void Enter()
    {
       
    }

    public virtual void Update()
    {
       
    }

    public virtual void Exit()
    {

    }
}
