using TMPro;
using UnityEngine;
using UnityEngine.UI; // UI 요소를 제어하려면 이 네임스페이스가 필수다
using GameData.Types;

public class SkillSlotManager : MonoBehaviour
{

    [SerializeField] private GameObject _skillSlotPrefab;
    [SerializeField] private Transform _horizonTransform;
    [SerializeField] private GameObject _verticalColumnPrefab;

    private Image[] _icons;
    private Button[] _buttons;
    private int _entityCount;


    private SkillController[] _controllers;


    public void InitializeBattleUI(RuntimeEntity[] players, SkillDatabase skillDatabase)
    {
        Debug.Log("UI 생성 시작");
        _entityCount = players.Length;
        _controllers = new SkillController[_entityCount];
        _buttons = new Button[4 * _entityCount];
        _icons = new Image[4 * _entityCount];

        // 1. 레이아웃 엔진 가동 (가로 배치)
        for (int c = 0; c < _entityCount; c++)
        {
            if (!players[c].isAlive) return;
            // 유닛당 1개의 세로 열 생성
            GameObject column = Instantiate(_verticalColumnPrefab, _horizonTransform);
            _controllers[c] = new SkillController(players[c].BaseData, skillDatabase);

            int[] currentIndex = _controllers[c].ShowSlot(); // 0 넥스트 1 2슬   2  1슬
          
            int defenseIndex = players[c].BaseData.Defense;

            // 2. 개별 열 내부에 4개의 슬롯 배치 및 데이터 주입
            for (int r = 0; r < 4; r++)
            {
                GameObject slotObj = Instantiate(_skillSlotPrefab, column.transform);
                int slotFlatIndex = (c * 4) + r;

                // 참조 캐싱 (하드웨어 접근 최적화)
                _buttons[slotFlatIndex] = slotObj.GetComponent<Button>();
                _icons[slotFlatIndex] = slotObj.transform.Find("Icon").GetComponent<Image>();

                // 이벤트 바인딩 (클로저 캡처 주의)
                int entityIdx = c;
                int rowIdx = r;

                // 데이터 인출 및 스프라이트 적용
                int skillIndex = (r < 3) ? currentIndex[r] : defenseIndex; 
                var skillData = _controllers[c].GetSkillByIndex(skillIndex);

                _buttons[slotFlatIndex].onClick.AddListener(() => OnCellSelected(entityIdx, rowIdx));
                _icons[slotFlatIndex].sprite = skillData.skillIcon;
            }
        }
    }
    public void SetSprite(int entityIdx, int rowIdx, Sprite icon) //슬롯별 이미지 할당
    {
        int index = (entityIdx * 4) + rowIdx;

        if (index >= 0 && index < _icons.Length)
        {
            _icons[index].sprite = icon;
        }
    }

    private void OnCellSelected(int entityIdx, int rowIdx)
    {
        if (rowIdx >= 3) return;

        // 시각적 피드백 (예: 버튼 비활성화나 하이라이트)
        // _buttons[(entityIdx * 4) + rowIdx].interactable = false;

        // BattleManager의 선택 풀(Pool)에 데이터 전달 (이 부분은 인터페이스나 액션으로 처리)

        //switch (rowIdx)
        //{
        //    case SlotType.
        //}    
        Debug.Log($"캐릭터 {entityIdx}의 {rowIdx}번 슬롯 선택됨. 풀에 저장 대기.");
    }

}

