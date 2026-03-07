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

    [SerializeField] private EntityDatabase[] _playerEntitiyDatas;
    [SerializeField] private EntityDatabase[] _enemyEntitiyDatas;
    private RuntimeEntity[] _runtimePlayerDatas;
    private RuntimeEntity[] _runtimeEnemyDatas;
    private int[] _playerOrder;
    private int[] _enemyOrder;
    //private int _activePlayerIndex;
    private int _playerIndex;
    private int _enemyIndex;


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

        _spawner.InitHeap(_playerEntitiyDatas.Length + _enemyEntitiyDatas.Length);

        _playerIndex = _playerEntitiyDatas.Length;
        _enemyIndex = _enemyEntitiyDatas.Length;

        _playerOrder = new int[_playerIndex];
        _enemyOrder = new int[_enemyIndex];

        _runtimePlayerDatas = new RuntimeEntity[_playerIndex];
        _runtimeEnemyDatas = new RuntimeEntity[_enemyIndex];
    }

    private void Start()
    {
        _spawner.LoadStageData(ref _runtimePlayerDatas, _playerEntitiyDatas, _playerOrder.Length);
        _spawner.LoadStageData(ref _runtimeEnemyDatas, _enemyEntitiyDatas, _enemyOrder.Length);
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
            _runtimePlayerDatas[i].currentSpeed = BattleLogic.Roll(_runtimePlayerDatas[i]);
            
        }
        for (int i = 0; i < _enemyIndex; i++)
        {
            if(_runtimeEnemyDatas[i].isAlive == false) continue;
            _runtimeEnemyDatas[i].currentSpeed = BattleLogic.Roll(_runtimeEnemyDatas[i]);
        }
    }
}

