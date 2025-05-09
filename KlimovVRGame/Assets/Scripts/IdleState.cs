using UnityEngine;

public class IdleState : IState
{
    private NPCController npc;
    private float idleDuration;
    private float idleTimer;

    public IdleState(NPCController npc, float duration)
    {
        this.npc = npc;
        this.idleDuration = duration;
    }

    public void Enter()
    {
        npc.animator.SetBool("isRunning", false);
        npc.animator.SetBool("isWalking", false);
        idleTimer = 0f;
        Debug.Log("NPC вошел в состояние ожидания");
    }

    public void Update()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            npc.StateMachine.ChangeState(new PatrolState(npc));
        }
    }

    public void Exit()
    {
        Debug.Log("NPC выходит из состояния ожидания");
    }
}
