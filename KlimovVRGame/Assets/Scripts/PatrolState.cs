using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private NPCController npc;
    private Transform[] patrolPoints;
    private int currentPointIndex = 0;
    private float speed = 2f;
    private float arriveThreshold = 0.2f;

    public PatrolState(NPCController npc)
    {
        this.npc = npc;
        this.patrolPoints = npc.PatrolPoints;
    }

    public void Enter()
    {
        npc.SetAnimation("Walk");
        Debug.Log("NPC начал патрулирование");
    }

    public void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            npc.StateMachine.ChangeState(new IdleState(npc));
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - npc.transform.position).normalized;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        npc.transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(npc.transform.position, targetPoint.position);
        if (distance < arriveThreshold)
        {
            currentPointIndex++;

            // если обошёл все точки — вернуться в Idle
            if (currentPointIndex >= patrolPoints.Length)
            {
                npc.StateMachine.ChangeState(new IdleState(npc, 10f)); // пауза 10 сек
            }
        }
    }

    public void Exit()
    {
        Debug.Log("NPC закончил патрулирование");
    }
}
