using NUnit.Framework.Internal;
using UnityEngine;

public class SkillSlotLogic
{
    public int[] currentSlots = new int[3];
    private SkillShuffler _shuffler;

    public SkillSlotLogic(SkillShuffler shuffler)
    {
        _shuffler = shuffler;
        currentSlots[0] = _shuffler.Pop();
        currentSlots[1] = _shuffler.Pop();
        currentSlots[2] = _shuffler.Pop();

        Debug.Log("½½·Ô ÃÊ±âÈ­ ¿Ï·á 1¹ø ½½·Ô : " + currentSlots[0] + " 2¹ø ½½·Ô : " + currentSlots[1] + " ´ÙÀ½ ½½·Ô : " + currentSlots[2]);
    }
    public int[] ShowSlots() => currentSlots;

    public int SelectSlot(int usedSlotIndex)
    {
        int temp;
        temp = (usedSlotIndex == 1) ? currentSlots[0] : currentSlots[1];
        return temp;
    }

    public void RefillSlots(int usedSlotIndex)
    {
        if (usedSlotIndex == 1)
        {
            currentSlots[0] = currentSlots[1];
            currentSlots[1] = currentSlots[2];
        }
        else if (usedSlotIndex == 2) currentSlots[1] = currentSlots[2];

        currentSlots[2] = _shuffler.Pop();

        Debug.Log("½½·Ô ¸®ÇÊ ¿Ï·á 1¹ø ½½·Ô : " + currentSlots[0] + " 2¹ø ½½·Ô : " + currentSlots[1] + " ´ÙÀ½ ½½·Ô : " + currentSlots[2]);
    }

}
