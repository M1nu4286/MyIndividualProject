using TMPro;
using UnityEngine;
using UnityEngine.UI; // UI 요소를 제어하려면 이 네임스페이스가 필수다
using GameData.Types;

public class SkillSlotManager : MonoBehaviour
{
    //[System.Serializable]
    //public struct SkillCellUI
    //{
    //    public Image _icon;
    //    public Button _button;
    //}

    [SerializeField] private GameObject _columnPrefab;
    [SerializeField] private Transform _contentParent;

    private Image[] _icons;
    private Button[] _buttons;
    private int _entityCount;
    //private SkillCellUI[] _allCells;

    private SkillController[] _controllers;
    public void Setup(RuntimeEntity[] runtimeEntities, SkillDatabase skillDatabase)
    {
        _entityCount = runtimeEntities.Length;
        _controllers = new SkillController[_entityCount]; // 배열 할당
        _buttons = new Button[4 * _entityCount];
        _icons = new Image[4 * _entityCount];

        for (int c = 0; c < _entityCount; c++)
        {
            _controllers[c] = new SkillController(runtimeEntities[c].BaseData, skillDatabase);

            int[] currentHashes = _controllers[c].ShowSlot(); //프로필 제외 스킬 3칸 저장
            int defenseHash = runtimeEntities[c].BaseData.Defense;

            for (int r = 0; r < 4; r++)
            {

                GameObject slot = Instantiate(_columnPrefab, _contentParent);
                int index = (c * 4) + r;

                _buttons[index] = slot.GetComponent<Button>();
                _icons[index] = slot.transform.Find("Icon").GetComponent<Image>();

                int entityIdx = c;
                int rowIdx = r;

                _buttons[index].onClick.AddListener(() => OnCellSelected(entityIdx, rowIdx));

                int targetHash = (r < 3) ? currentHashes[r] : defenseHash;
                Sprite icon = _controllers[c].GetSkills(targetHash).skillIcon;
                SetSprite(entityIdx, r, icon);
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
        Debug.Log($"캐릭터 {entityIdx}의 {rowIdx}번 슬롯 선택됨. 풀에 저장 대기.");
    }

}

