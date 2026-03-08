using GameData.Types;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : MonoBehaviour
{
    private SkillActionHandler _actionHandler;
    private SkillShuffler _shuffler;
    private SkillSlotManager _slotManager;
    private int _slotIndex;
    public SkillEntry skillData;

    private void Awake()
    {
        _actionHandler = new SkillActionHandler();
        _shuffler = new SkillShuffler();
        _slotManager = new SkillSlotManager(_shuffler);
    }

    void Start()
    {
    }

    private void Update()
    {


    }


    [ContextMenu("First Skill")]
    private void FirstSkill()
    {
        _actionHandler.HandleAction(gameObject, _slotManager.slot1);
        _slotIndex = 1;
    }
    [ContextMenu("Second Skill")]
    private void SecondSkill()
    {
        _actionHandler.HandleAction(gameObject, _slotManager.slot2);
        _slotIndex = 2;
    }
    [ContextMenu("Select End")]
    private void SelectEnd()
    {
        _actionHandler.ActionStart();
        _slotManager.RefillSlots(_slotIndex);
    }
}
