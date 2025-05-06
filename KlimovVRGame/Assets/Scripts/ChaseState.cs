using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private NPCController npc;
    private Transform target;
    private float chaseSpeed = 3.5f;
    private float attackRange = 2f;

    public ChaseState(NPCController npc, Transform target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        npc.SetAnimation("Run");
        Debug.Log("NPC начал преследование цели");
    }

    public void Update()
    {
        if (target == null)
        {
            npc.StateMachine.ChangeState(new IdleState(npc));
            return;
        }

        Vector3 direction = (target.position - npc.transform.position).normalized;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        npc.transform.position += direction * chaseSpeed * Time.deltaTime;

        float distance = Vector3.Distance(npc.transform.position, target.position);

        if (distance <= attackRange)
        {
            npc.StateMachine.ChangeState(new AttackState(npc, target));
        }
    }

    public void Exit()
    {
        Debug.Log("NPC прекратил преследование");
    }
}
