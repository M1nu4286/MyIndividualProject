using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameData.Types;

public class SkillSlotManager : MonoBehaviour
{
    [SerializeField] private GameObject _skillSlotPrefab;
    [SerializeField] private Transform _horizonTransform;
    [SerializeField] private GameObject _verticalColumnPrefab;
    [SerializeField] private GameObject _profileSlotPrefab; // 프로필 행 전용 프리팹 (없으면 _skillSlotPrefab 사용)

    private Image[] _icons;
    private Button[] _buttons;
    private int _entityCount;

    private SkillController[] _controllers;

    // 열당 슬롯 수: Profile(0), FirstSlot(1), SecondSlot(2), NextSlot(3)
    private const int SLOTS_PER_ENTITY = 4;

    public void InitializeBattleUI(RuntimeEntity[] players, SkillDatabase skillDatabase)
    {
        Debug.Log("UI 초기화 완료");
        _entityCount = players.Length;
        _controllers = new SkillController[_entityCount];
        _buttons = new Button[SLOTS_PER_ENTITY * _entityCount];
        _icons = new Image[SLOTS_PER_ENTITY * _entityCount];

        for (int c = 0; c < _entityCount; c++)
        {
            if (!players[c].isAlive) continue;

            // 플레이어마다 수직 열 생성
            GameObject column = Instantiate(_verticalColumnPrefab, _horizonTransform);
            _controllers[c] = new SkillController(players[c].BaseData, skillDatabase);

            // [0]=1번슬롯(조작가능), [1]=2번슬롯(조작가능), [2]=다음슬롯(미리보기)
            int[] currentSlots = _controllers[c].ShowSlot();

            for (int r = 0; r < SLOTS_PER_ENTITY; r++)
            {
                SlotType slotType = (SlotType)r;

                // 프로필 행은 전용 프리팹 사용 (없으면 스킬슬롯 프리팹으로 대체)
                GameObject prefabToUse = (slotType == SlotType.Profile && _profileSlotPrefab != null)
                    ? _profileSlotPrefab
                    : _skillSlotPrefab;

                GameObject slotObj = Instantiate(prefabToUse, column.transform);
                int slotFlatIndex = (c * SLOTS_PER_ENTITY) + r;

                _buttons[slotFlatIndex] = slotObj.GetComponent<Button>();
                _icons[slotFlatIndex] = slotObj.transform.Find("Icon").GetComponent<Image>();

                int entityIdx = c;
                int rowIdx = r;

                switch (slotType)
                {
                    case SlotType.Profile:
                        _icons[slotFlatIndex].sprite = players[c].BaseData.profileIcon;
                        _buttons[slotFlatIndex].interactable = false;
                        break;

                    case SlotType.FirstSlot:
                        var skill1 = _controllers[c].GetSkillByIndex(currentSlots[0]);
                        _icons[slotFlatIndex].sprite = skill1.skillIcon;
                        _buttons[slotFlatIndex].onClick.AddListener(() => OnCellSelected(entityIdx, rowIdx));
                        break;

                    case SlotType.SecondSlot:
                        var skill2 = _controllers[c].GetSkillByIndex(currentSlots[1]);
                        _icons[slotFlatIndex].sprite = skill2.skillIcon;
                        _buttons[slotFlatIndex].onClick.AddListener(() => OnCellSelected(entityIdx, rowIdx));
                        break;

                    case SlotType.NextSlot:
                        var nextSkill = _controllers[c].GetSkillByIndex(currentSlots[2]);
                        _icons[slotFlatIndex].sprite = nextSkill.skillIcon;
                        _buttons[slotFlatIndex].interactable = false;
                        break;
                }
            }
        }
    }

    public void SetSprite(int entityIdx, int rowIdx, Sprite icon)
    {
        int index = (entityIdx * SLOTS_PER_ENTITY) + rowIdx;
        if (index >= 0 && index < _icons.Length)
            _icons[index].sprite = icon;
    }

    private void OnCellSelected(int entityIdx, int rowIdx)
    {
        SlotType slotType = (SlotType)rowIdx;
        if (slotType != SlotType.FirstSlot && slotType != SlotType.SecondSlot) return;

        Debug.Log($"캐릭터 {entityIdx}의 {slotType} 스킬 선택됨. 풀에 넣기 예정.");
    }
}
