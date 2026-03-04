using UnityEngine;

public class Entity : MonoBehaviour
{
    public RuntimeEntity enittyData;
    protected StateMachine _stateMachine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _stateMachine = new StateMachine();
    }
}
