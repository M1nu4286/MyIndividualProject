using UnityEngine;

public class SkillShuffler
{
    private int[] Slots;
    public SkillShuffler(int[] Slots) 
    {
        this.Slots = Slots;
        Shuffle();
    }
    private int _currentIndex = 0;


    public int[] Shuffle()
    {
        for (int i = Slots.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = Slots[j];
            Slots[j] = Slots[i];
            Slots[i] = temp;
        }
        _currentIndex = 0;
        return Slots;
    }
    public int Pop()
    {

        if (_currentIndex >= Slots.Length)
        {
            Shuffle();
        }

        return Slots[_currentIndex++];
    }
}
