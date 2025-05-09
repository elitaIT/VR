using System.Collections;
using UnityEngine;

public class PatrolState : IState
{
    private NPCController npc;
    private Transform[] patrolPoints;
    private int currentPointIndex;
    private float speed = 2f;
    private float arriveThreshold = 0.2f;

    public PatrolState(NPCController npc)
    {
        this.npc = npc;
        this.patrolPoints = npc.PatrolPoints;
        this.currentPointIndex = 0;
    }

    public void Enter()
    {
        npc.animator.SetBool("isWalking", true);
        npc.animator.SetBool("isRunning", false);
        Debug.Log("NPC начал патрулирование");
    }

    public void Update()
    {
        if (npc.isAttacking) return;
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            npc.StateMachine.ChangeState(new IdleState(npc, 10f));
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = targetPoint.position - npc.transform.position;
        Vector3 moveDir = direction.normalized;

        // Плавный поворот к точке
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            npc.transform.rotation = Quaternion.Slerp(
                npc.transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }

        // Перемещение NPC вручную
        npc.transform.position += moveDir * speed * Time.deltaTime;

        // Проверка расстояния до цели
        if (direction.magnitude < arriveThreshold)
        {
            currentPointIndex++;

            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = 0; // Сброс к началу маршрута
                npc.StateMachine.ChangeState(new IdleState(npc, 10f)); // пауза
            }
        }
    }

    public void Exit()
    {
        Debug.Log("NPC закончил патрулирование");
        npc.SetAnimation("Idle");
    }
}
