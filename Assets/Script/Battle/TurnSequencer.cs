using UnityEngine;

public class TurnSequencer : MonoBehaviour
{
    [SerializeField] private SkillSlotManager _slotManager;
    private SkillDatabase _skillDb;
    private EntityDatabase _entityDb;

    public void InitHash(EntityDatabase entityDatabase,SkillDatabase skillDatabase)
    {
        _entityDb = entityDatabase;
        _skillDb = skillDatabase;
    }

    public void StartBattle(RuntimeEntity[] players)
    {
        // 2. 시퀀서가 모든 주도권을 쥐고 UI를 띄움
        // 여기서 SkillSlotManager에게 필요한 모든 자원을 주입(Injection)한다.
        _slotManager.Setup(players, _skillDb);
    }
}