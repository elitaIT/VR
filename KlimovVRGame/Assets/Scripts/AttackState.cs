using UnityEngine;

public class AttackState : IState
{
    private NPCController npc;
    private Transform target;
    private float attackRange = 0.1f;
    private float attackCooldown = 1.5f;
    private float cooldownTimer = 0f;

    public AttackState(NPCController npc, Transform target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        cooldownTimer = 0f;
        npc.animator.SetTrigger("Attack");
        Debug.Log("NPC вошёл в состояние атаки");
    }

    public void Update()
    {
        
        if (target == null)
        {
            npc.StateMachine.ChangeState(new IdleState(npc, 10f));
            return;
        }

        // Поворот к цели
        Vector3 direction = (target.position - npc.transform.position).normalized;
        npc.transform.rotation = Quaternion.Slerp(
            npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 5f
        );
        
        float distance = Vector3.Distance(npc.transform.position, target.position);

        if (distance > attackRange)
        {
            npc.StateMachine.ChangeState(new ChaseState(npc, target));
            return;
        }

        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            npc.animator.SetTrigger("Attack");
            Attack();
            cooldownTimer = 0f;
        }
    }

    private void Attack()
    {
        Debug.Log("NPC атакует цель!");
        // Здесь можно вызвать урон цели, например:
        // target.GetComponent<Health>()?.TakeDamage(10);
    }

    public void Exit()
    {
        Debug.Log("NPC выходит из состояния атаки");
    }
}
