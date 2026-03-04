using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    ActionExecutionor _actionExecutionor;
    TurnSequencer _turnSequencer;
    Spawner _spawner;
    StateMachine _stateMachine;
    [SerializeField] private EntityData[] _playerEntitiyDatas;
    [SerializeField] private EntityData[] _enemyEntitiyDatas;
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
        _stateMachine.Initialize(new BattleStartState(_stateMachine, this));
        _spawner.LoadStageData(ref _runtimePlayerDatas, _playerEntitiyDatas, _playerOrder.Length);
        _spawner.LoadStageData(ref _runtimeEnemyDatas, _enemyEntitiyDatas, _enemyOrder.Length);
        StartTurn();
    }
    [ContextMenu("Start")]
    private void StartTurn()
    {
        _turnSequencer.TurnStart(_runtimePlayerDatas);
        _turnSequencer.TurnStart(_runtimeEnemyDatas);
        RollSpeed();
        _spawner.GetSortOrder(ref _playerOrder, _playerIndex, _runtimePlayerDatas);
        _spawner.GetSortOrder(ref _enemyOrder, _enemyIndex, _runtimeEnemyDatas);
        _actionExecutionor.BattleLog(_playerOrder, _enemyOrder, _runtimePlayerDatas, _runtimeEnemyDatas);

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

