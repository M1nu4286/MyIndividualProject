using GameData.Types;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityDatabase : ScriptableObject
{
    public EntityEntry[] Entities;
    private Dictionary<int, int> _HashToIndex;

    public void Initialize()
    {
        _HashToIndex = new Dictionary<int, int>(Entities.Length);

        for (int i = 0; i < Entities.Length; i++)
        {
            int idHash = Entities[i].EntityIDHash;
            if (!_HashToIndex.ContainsKey(idHash))
            {
                _HashToIndex[idHash] = i;
            }
            else
            {
                Debug.LogError($"{i} 해시 충돌.");
            }
        }
    }

    public EntityEntry GetEntity(int targetHash)
    {
        if (_HashToIndex == null) Initialize();

        if (_HashToIndex.TryGetValue(targetHash, out int index))
        {
            // 배열의 해당 인덱스에 직접 접근 (Pointer-like behavior)
            return Entities[index];
        }

        Debug.LogError($"ID {targetHash}를 찾을 수 없습니다.");
        return default;
    }
}
