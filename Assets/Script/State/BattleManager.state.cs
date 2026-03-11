using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    public abstract class TotalState
    {
        protected BattleManager _owner;
        protected StateMachine _stateMachine;

        public TotalState(StateMachine stateMachine, BattleManager owner)
        {
            _stateMachine = stateMachine;
            _owner = owner;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }

    public class BattleStartState : TotalState
    {
        public BattleStartState(StateMachine stateMachine, BattleManager owner) : base(stateMachine, owner)
        {
        }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Battle Start State: Initializing battle...");

            _owner.RollSpeed();
            _owner._spawner.GetSortOrder(ref _owner._entityOrder, _owner._entityIndex, _owner._runtimeEntityDatas);

            //_owner._actionExecutionor.BattleLog(_owner._entityOrder, _owner._entityIndex, _owner._runtimeEntityDatas);
            _stateMachine.ChangeState(_owner._statePool[(int)BattleState.EvaluationState]);
            
        }
        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exiting Battle Start State.");
        }
    }
    public class EvaluationState : TotalState //플레이어 조작 단계
    {
        public EvaluationState(StateMachine stateMachine, BattleManager owner) : base(stateMachine, owner)
        {
        }
        public override void Enter()
        {
            Debug.Log("Evaluation State: Evaluating actions...");
            int currentActorIdx = _owner._entityOrder[0];
            _owner.GetTurnSequencer().ProcessTurn(currentActorIdx, _owner._runtimeEntityDatas);
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exiting Evaluation State.");
        }
    }

    public class HapAndClashState : TotalState //순수 합 계산
    {
        public HapAndClashState(StateMachine stateMachine, BattleManager owner) : base(stateMachine, owner)
        {
        }
    }

    public class ActionExecutionState : TotalState //애니메이션 재생 단계
    {
        public ActionExecutionState(StateMachine stateMachine, BattleManager owner) : base(stateMachine, owner)
        {
        }
    }

    public class CleanUpState : TotalState
    {
        public CleanUpState(StateMachine stateMachine, BattleManager owner) : base(stateMachine, owner)
        {
        }
    }
}
