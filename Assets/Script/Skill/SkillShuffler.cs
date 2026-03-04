using UnityEngine;

public class SkillShuffler
{
    private readonly SkillType[] _cycle = new SkillType[6];
    private int _currentIndex = 0;

    public SkillShuffler()
    {
        _cycle[0] = SkillType.Skill_1;
        _cycle[1] = SkillType.Skill_1;
        _cycle[2] = SkillType.Skill_1;
        _cycle[3] = SkillType.Skill_2;
        _cycle[4] = SkillType.Skill_2;
        _cycle[5] = SkillType.Skill_3;

        Shuffle();
    }

    private void Shuffle()
    {
        for (int i = _cycle.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            SkillType temp = _cycle[j];
            _cycle[j] = _cycle[i];
            _cycle[i] = temp;
        }
        _currentIndex = 0;
    }
    public SkillType Pop() {

        if (_currentIndex >= _cycle.Length) 
        {
            Shuffle();
        }

        return _cycle[_currentIndex++];
    }
}
