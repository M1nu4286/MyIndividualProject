using UnityEngine;

public class SkillActionHandler
{
    public SkillType SelectedSkill;
    public void HandleAction(GameObject player,SkillType skill)
    {
        Debug.Log(player.name +"Select Skill : " + skill);
        SelectedSkill = skill;
    }

    public void ActionStart() 
    {
        switch (SelectedSkill)
            {
                case SkillType.None:
                    break;
                case SkillType.Skill_1:
                    Debug.Log(SelectedSkill + ". 1스킬 시전");
                    break;
                case SkillType.Skill_2:
                    Debug.Log(SelectedSkill + ". 2스킬 시전");
                    break;
                case SkillType.Skill_3:
                    Debug.Log(SelectedSkill+"!! 3스킬 시전");
                    break;
                case SkillType.Guard:
                    Debug.Log("Guard!");
                    break;
        }
    }
}
