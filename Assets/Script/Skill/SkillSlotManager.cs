using UnityEngine;

public class SkillSlotManager
{
    public SkillType slot1;
    public SkillType slot2;
    public SkillType nextSlot;
    private SkillShuffler _shuffler;

    public SkillSlotManager(SkillShuffler shuffler)
    {
        _shuffler = shuffler;
        slot1 = _shuffler.Pop();
        slot2 = _shuffler.Pop();
        nextSlot = _shuffler.Pop();

        Debug.Log("½½·Ô ÃÊ±âÈ­ ¿Ï·á 1¹ø ½½·Ô : " + slot1 + " 2¹ø ½½·Ô : " + slot2 + " ´ÙÀ½ ½½·Ô : " + nextSlot);
    }


    public void RefillSlots(int usedSlotIndex) 
    {
        if (usedSlotIndex == 1) 
        {
            slot1 = slot2;
            slot2 = nextSlot;
        }
        else if (usedSlotIndex == 2) slot2 = nextSlot;

        nextSlot = _shuffler.Pop();

        Debug.Log("½½·Ô ¸®ÇÊ ¿Ï·á 1¹ø ½½·Ô : " + slot1 + " 2¹ø ½½·Ô : " + slot2 + " ´ÙÀ½ ½½·Ô : " + nextSlot);
    }

}
