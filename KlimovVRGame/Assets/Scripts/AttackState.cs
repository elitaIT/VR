using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private NPCController npc;
    private Transform target;
    private float attackRange = 2f; // Дистанция атаки
    private float attackCooldown = 1.5f; // Задержка между ударами
    private float cooldownTimer = 0f;

    public AttackState(NPCController npc, Transform target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        cooldownTimer = 0f;
        npc.SetAnimation("Attack"); // Запускаем анимацию атаки
        Debug.Log("NPC вошёл в состояние атаки");
    }

    public void Update()
    {
        if (target == null)
        {
            npc.StateMachine.ChangeState(new IdleState(npc));
            return;
        }

        // Повернуться к цели
        Vector3 direction = (target.position - npc.transform.position).normalized;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        float distance = Vector3.Distance(npc.transform.position, target.position);

        // Если цель слишком далеко — прекратить атаку
        if (distance > attackRange)
        {
            npc.StateMachine.ChangeState(new IdleState(npc)); // Переход к преследованию
            return;
        }

        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            Attack();
            cooldownTimer = 0f;
        }
    }

    private void Attack()
    {
        Debug.Log("NPC атакует цель!");
        // Здесь можно вызвать урон, например:
        // target.GetComponent<Health>()?.TakeDamage(10);
    }

    public void Exit()
    {
        Debug.Log("NPC выходит из состояния атаки");
    }

}
