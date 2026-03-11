using GameData.Types;
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
    public EntityType entityType;

    //유닛 데이터
    [SerializeField] private EntityDatabase _EntitiyDatas;
    [SerializeField] private SkillDatabase _skillDatas;

    //런타임데이터
    private RuntimeEntity[] _runtimeEntityDatas;
    private RuntimeEntity[] _runtimePlayerDatas;
    private RuntimeEntity[] _runtimeEnemyDatas;

    //객체별 개수
    private int _entityIndex;
    private int _playerIndex;
    private int _enemyIndex;


    //간접참조 배열
    private int[] _entityOrder;
    private int[] _playerOrder;
    private int[] _enemyrOrder;



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
        _turnSequencer.InitHash(_EntitiyDatas, _skillDatas);

        _playerIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Player);
        _enemyIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Enemy);
        _entityIndex = _EntitiyDatas.Entities.Length;

        _entityOrder = new int[_entityIndex];
        _runtimeEntityDatas = new RuntimeEntity[_entityIndex];


   
    }

    private void Start()
    {
        _spawner.LoadStageData(ref _runtimeEntityDatas, _EntitiyDatas.Entities, _entityIndex);
        SplitEntitiesByType();
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
        for (int i = 0; i < _entityIndex; i++)
        {
            if (_runtimeEntityDatas[i].isAlive == false) continue;
            _runtimeEntityDatas[i].currentSpeed = BattleLogic.Roll(_runtimeEntityDatas[i]);

        }
    }
    private void SplitEntitiesByType()
    {
        _runtimePlayerDatas = new RuntimeEntity[_playerIndex];
        _runtimeEnemyDatas = new RuntimeEntity[_enemyIndex];  // 필드 추가 필요

        int pIdx = 0, eIdx = 0;
        for (int i = 0; i < _entityIndex; i++)
        {
            if (_runtimeEntityDatas[i].BaseData.Type == GameData.Types.EntityType.Player)
                _runtimePlayerDatas[pIdx++] = _runtimeEntityDatas[i];
            else
                _runtimeEnemyDatas[eIdx++] = _runtimeEntityDatas[i];
        }
    }
}

