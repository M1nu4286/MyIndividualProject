using GameData.Types;
using System.Collections.Generic;
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
           
            Debug.Log("순서입력완료");
            List<int> sortedPlayerIndices = new List<int>();

            for (int i = 0; i < _owner._entityIndex; i++)
            {
                int entityIdx = _owner._entityOrder[i];
                if (_owner._runtimeEntityDatas[entityIdx].BaseData.Type == EntityType.Player &&
                    _owner._runtimeEntityDatas[entityIdx].isAlive)
                {
                    sortedPlayerIndices.Add(entityIdx);
                }
            }

            // 3. UI 매니저에게 정렬된 '플레이어'들만 전달
            //_owner._turnSequencer.ProcessTurn(sortedPlayerIndices.ToArray(), );

            if (_owner.GetSkillUI().IsInitialized == false)
            {
                _owner.GetTurnSequencer().ProcessTurn(sortedPlayerIndices.ToArray(), _owner._runtimeEntityDatas);
            }
            else
            {
                _owner.GetSkillUI().UpdateSkillUI(_owner._runtimeEntityDatas);
            }
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
