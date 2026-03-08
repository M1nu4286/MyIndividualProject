using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManager : MonoBehaviour
{
    private ActionExecutionor _actionExecutionor;
    private TurnSequencer _turnSequencer;
    private Spawner _spawner;
    private StateMachine _stateMachine;
    private TotalState[] _statePool;

    //유닛 데이터
    [SerializeField] private EntityDatabase _EntitiyDatas;
    [SerializeField] private SkillDatabase _skillDatas;
    
    //런타임데이터
    private RuntimeEntity[] _runtimePlayerDatas;
    private RuntimeEntity[] _runtimeEnemyDatas;
    
    //객체별 개수
    private int _playerIndex;
    private int _enemyIndex;

    //간접참조 배열
    private int[] _playerOrder;
    private int[] _enemyOrder;



    private void Awake()
    {
        _actionExecutionor = new ActionExecutionor();
        _turnSequencer = new TurnSequencer();
        _spawner = new Spawner();
        _stateMachine = new StateMachine();

        _statePool = new TotalState[(int)BattleState.CleanUpState + 1];
        _statePool[(int)BattleState.BattleStartState] = new BattleStartState(_stateMachine, this);
        _statePool[(int)BattleState.EvaluationState] = new EvaluationState(_stateMachine, this);
        _statePool[(int)BattleState.HapAndClashState] = new HapAndClashState(_stateMachine, this);
        _statePool[(int)BattleState.ActionExecutionState] = new ActionExecutionState(_stateMachine, this);
        _statePool[(int)BattleState.CleanUpState] = new CleanUpState(_stateMachine, this);

        _spawner.InitHeap(_EntitiyDatas.Entities.Length);

        _playerIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Player);
        _enemyIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Enemy);

        _playerOrder = new int[_playerIndex];
        _enemyOrder = new int[_enemyIndex];

        _runtimePlayerDatas = new RuntimeEntity[_playerIndex];
        _runtimeEnemyDatas = new RuntimeEntity[_enemyIndex];
    }

    private void Start()
    {
        _spawner.LoadStageData(ref _runtimePlayerDatas, _EntitiyDatas.Entities, _playerIndex, GameData.Types.EntityType.Player);
        _spawner.LoadStageData(ref _runtimeEnemyDatas, _EntitiyDatas.Entities, _enemyIndex, GameData.Types.EntityType.Enemy);
    }
    [ContextMenu("Start")]

    private void GameStart()
    {
        _stateMachine.Initialize(_statePool[(int)BattleState.BattleStartState]);
    }
    [ContextMenu("Start Turn")]
    private void StartTurn()
    {
    }

    [ContextMenu("Select Finish")]
    private void SelectFinish()
    {

    }

    private void RollSpeed()
    {
        for (int i = 0; i < _playerIndex; i++)
        {
            if (_runtimePlayerDatas[i].isAlive == false) continue;
            _runtimePlayerDatas[i].currentSpeed = BattleLogic.Roll(_runtimePlayerDatas[i],i);

        }
        for (int i = 0; i < _enemyIndex; i++)
        {
            if (_runtimeEnemyDatas[i].isAlive == false) continue;
            _runtimeEnemyDatas[i].currentSpeed = BattleLogic.Roll(_runtimeEnemyDatas[i],i);
        }
    }
}

