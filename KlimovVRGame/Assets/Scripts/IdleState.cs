using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private NPCController npc;
    private float waitTime;
    private float timer;

    public IdleState(NPCController npc, float waitTime = 10f)
    {
        this.npc = npc;
        this.waitTime = waitTime;
    }

    public void Enter()
    {
        timer = 0f;
        npc.SetAnimation("Idle");
        Debug.Log("Entering Idle State");
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            npc.StateMachine.ChangeState(new PatrolState(npc));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
