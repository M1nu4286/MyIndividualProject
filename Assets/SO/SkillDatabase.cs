using GameData.Types;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Scriptable Objects/SkillDatabase")]
// SkillDatabase.cs 권장 구조
public class SkillDatabase : ScriptableObject
{
    public SkillEntry[] Skills;
    
    private Dictionary<int, int> _hashToIndex;

    public void Initialize()
    {
        _hashToIndex = new Dictionary<int, int>(Skills.Length);
        for (int i = 0; i < Skills.Length; i++)
        {
            // SkillID도 해시로 관리
            _hashToIndex[Skills[i].SkillIDHash] = i;
        }
    }

    // 인덱스로 즉시 접근 (엔티티가 들고 있는 SkillIndex를 사용)
    public SkillEntry GetSkillByIndex(int index) => Skills[index];

    // 해시로 검색 (특정 스킬 ID로 정보를 찾을 때)
    public SkillEntry GetSkillByHash(int hash)
    {
        if (_hashToIndex.TryGetValue(hash, out int index)) return Skills[index];
        return default;
    }

}
