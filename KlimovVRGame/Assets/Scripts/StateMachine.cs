using UnityEngine;

public class StateMachine
{
    private IState currentState;
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}