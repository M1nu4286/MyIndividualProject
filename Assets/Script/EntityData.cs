using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/EntityData")]
public class EntityData: ScriptableObject
{
    public string entityID;
    public int maxHp;
    public int maxMental;
    public int minSpeed;
    public int maxSpeed;

}
