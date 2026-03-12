using GameData.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public partial class BattleManager : MonoBehaviour
{
    //부품 선언
    private ActionExecutionor _actionExecutionor;
    private TurnSequencer _turnSequencer;
    private Spawner _spawner;
    private StateMachine _stateMachine;
    private TotalState[] _statePool;
    public EntityType entityType;

    //유닛 데이터
    [SerializeField] private SkillSlotManager _skillSlotUI;
    [SerializeField] private EntityDatabase _EntitiyDatas;
    [SerializeField] private SkillDatabase _skillDatas;

    //런타임데이터
    private RuntimeEntity[] _runtimeEntityDatas;

    //객체별 개수
    private int _entityIndex;
    private int _playerIndex;
    private int _enemyIndex;


    //간접참조 배열
    private int[] _entityOrder;


    private void Awake()
    {
        //_turnSequencer = new TurnSequencer();

        _turnSequencer = GetComponent<TurnSequencer>();
        _actionExecutionor = new ActionExecutionor();
        _spawner = new Spawner();
        _stateMachine = new StateMachine();

        if (_skillSlotUI == null) Debug.LogError("SkillSlotManager is NOT assigned in BattleManager!");
        if (_skillDatas == null) Debug.LogError("SkillDatabase is NOT assigned in BattleManager!");
        //턴시퀀서에 객체,스킬,UI정보 넘기기
        _turnSequencer.InitHash(_EntitiyDatas, _skillDatas, _skillSlotUI);

        //FSM state 선언
        _statePool = new TotalState[(int)BattleState.CleanUpState + 1];
        _statePool[(int)BattleState.BattleStartState] = new BattleStartState(_stateMachine, this);
        _statePool[(int)BattleState.EvaluationState] = new EvaluationState(_stateMachine, this);
        _statePool[(int)BattleState.HapAndClashState] = new HapAndClashState(_stateMachine, this);
        _statePool[(int)BattleState.ActionExecutionState] = new ActionExecutionState(_stateMachine, this);
        _statePool[(int)BattleState.CleanUpState] = new CleanUpState(_stateMachine, this);

        //속도 리롤 우선순위 큐 위한 힙 길이 할당
        _spawner.InitHeap(_EntitiyDatas.Entities.Length);



        _entityIndex = _EntitiyDatas.Entities.Length;
        _entityOrder = new int[_entityIndex];

        _runtimeEntityDatas = new RuntimeEntity[_entityIndex];
        _playerIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Player);
        _enemyIndex = BattleLogic.IndexCreator(_EntitiyDatas, GameData.Types.EntityType.Enemy);

    }

    private void Start()
    {
        _spawner.LoadStageData(ref _runtimeEntityDatas, _EntitiyDatas.Entities, _entityIndex);
  
    }

    private void Update()
    {
        if (_stateMachine.currentState != null)
            _stateMachine.UpdateActiveState();
    }


    public TurnSequencer GetTurnSequencer() => _turnSequencer;
    public SkillSlotManager GetSkillUI() => _skillSlotUI;


    [ContextMenu("Game Start")]

    private void GameStart()
    {
        _stateMachine.Initialize(_statePool[(int)BattleState.BattleStartState]);
    }


    private void RollSpeed()
    {
        for (int i = 0; i < _entityIndex; i++)
        {
            if (_runtimeEntityDatas[i].isAlive == false) continue;
            _runtimeEntityDatas[i].currentSpeed = BattleLogic.Roll(_runtimeEntityDatas[i]);

        }
    }
    //private void SplitDatas() 
    //{
    //    int pIdx = 0;
    //    int eIdx = 0;
    //    for (int i = 0; i < _runtimeEntityDatas.Length ; i++)
    //    {
    //        if (_runtimeEntityDatas[i].BaseData.Type == GameData.Types.EntityType.Player)
    //        {
    //            _runtimePlayerDatas[i].index= pIdx;
    //            _runtimePlayerDatas[i].refData= i;

    //            pIdx++;
    //        }
    //        if (_runtimeEntityDatas[i].BaseData.Type == GameData.Types.EntityType.Enemy)
    //        {
    //            _runtimeEnemyDatas[eIdx].index = eIdx;
    //            _runtimeEnemyDatas[eIdx].refData= i;
    //            eIdx++;
    //        }
    //    }
    //}
   
}

