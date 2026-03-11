using GameData.Types;
using UnityEngine;

public class TurnSequencer : MonoBehaviour
{
    private SkillSlotManager _slotManager;
    private SkillDatabase _skillDb;
    private EntityDatabase _entityDb;

    public void InitHash(EntityDatabase entityDatabase,SkillDatabase skillDatabase, SkillSlotManager skillSlotManager)
    {
        _entityDb = entityDatabase;
        _skillDb = skillDatabase;
        _slotManager = skillSlotManager;
    }

    public void ProcessTurn(int actorIndex, RuntimeEntity[] allEntities)
    {
        RuntimeEntity currentActor = allEntities[actorIndex];

        // 1. 하드웨어 관점의 상태 검증 (살아있는지 확인)
        if (!currentActor.isAlive) return;

        // 2. 화면 조작 주도: 플레이어인 경우에만 UI를 띄우도록 명령한다.
        if (currentActor.BaseData.Type == EntityType.Player)
        {
            Debug.Log($"[TurnSequencer] 플레이어 {currentActor.BaseData.Editor_ID}의 차례. UI 출력을 시작한다.");

            // 기존의 BattleManager를 거치지 않고 직접 UI의 Setup을 당긴다.
            // 단일 유닛의 조작을 위해 배열 형태로 감싸서 전달한다.
            _slotManager.Setup(new RuntimeEntity[] { currentActor }, _skillDb);
        }
        else
        {
            Debug.Log($"[TurnSequencer] 적 {currentActor.BaseData.Editor_ID}의 차례. UI 출력을 생략한다.");
            // 적 AI 로직 실행 트리거 (필요 시)
        }
    }
}