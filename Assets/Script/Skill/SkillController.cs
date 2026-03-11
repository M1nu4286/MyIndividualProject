using GameData.Types;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController
{
    private int[] SkillHashs = new int[6];
    private SkillActionHandler _actionHandler;
    private SkillShuffler _shuffler;
    private SkillSlotLogic _slotLogic;
    private SkillDatabase _database;
    public SkillController(EntityEntry Entity, SkillDatabase database)
    {
        SkillHashs[0] = Entity.SkillIndex1;
        SkillHashs[1] = Entity.SkillIndex1;
        SkillHashs[2] = Entity.SkillIndex1;
        SkillHashs[3] = Entity.SkillIndex2;
        SkillHashs[4] = Entity.SkillIndex2;
        SkillHashs[5] = Entity.SkillIndex3;

        _database = database;
        _actionHandler = new SkillActionHandler();
        _shuffler = new SkillShuffler(SkillHashs);
        _slotLogic = new SkillSlotLogic(_shuffler);
    }

    private int _reservedSlotIndex = 0;

    public struct SkillSelection
    {
        public int entityIdx;
        public int rowIdx;
        public int skillHash;
    }
    public void Commit()
    {
        if (_reservedSlotIndex == 0) return;

        // 실제 논리 계층의 데이터 전이 실행
        _slotLogic.RefillSlots(_reservedSlotIndex);

        // 예약 정보 초기화
        _reservedSlotIndex = 0;
    }


    public int Reserve(int rowIdx)
    {
        _reservedSlotIndex = (rowIdx == 2) ? 1 : 2;

        return _slotLogic.currentSlots[_reservedSlotIndex - 1];
    }
    public int[] ShowSlot() //사용가능 슬롯 2개 & 다음 슬롯 1개 표시 int(hash)값으로 넘겨서 배틀매니저에서 이미 만든 해시값->스킬엔트리 딕셔너리 메서드를 사용해서 즉석으로 스킬 정보 뽑아서 UI 버튼 색상 및 스킬 정보 표시
    {
        return _slotLogic.ShowSlots();
    }

    private void Shffle() //자동화 셔플
    {
        SkillHashs = _shuffler.Shuffle();
    }

    public SkillEntry GetSkills(int CurrentHashs)
    {
        //int CurrentHashs = ShowSlot();

        return _database.GetSkillByHash(CurrentHashs);
    }

    public void Cancel() => _reservedSlotIndex = 0;

    public int[] GetCurrentDisplayHashes() => _slotLogic.ShowSlots();
    public SkillEntry GetSkillData(int hash) => _database.GetSkillByHash(hash);
}
