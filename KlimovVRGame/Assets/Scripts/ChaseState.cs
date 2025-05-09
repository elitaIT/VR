using UnityEngine;

public class ChaseState : IState
{
    private NPCController npc;
    private Transform target;
    private float speed = 3.5f;

    public ChaseState(NPCController npc, Transform target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        npc.animator.SetBool("isRunning", true);
        npc.animator.SetBool("isWalking", false);
        Debug.Log("NPC начал погоню");
    }

    public void Update()
    {
        if (npc.isAttacking) return;
        if (target == null)
        {
            npc.StateMachine.ChangeState(new IdleState(npc, 5f));
            return;
        }

        Vector3 direction = target.position - npc.transform.position;
        Vector3 moveDir = direction.normalized;
        
        if (moveDir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDir);
            npc.transform.rotation = Quaternion.Slerp(
                npc.transform.rotation,
                lookRotation,
                Time.deltaTime * 5f
            );
        }

        npc.transform.position += moveDir * speed * Time.deltaTime;

        float distance = Vector3.Distance(npc.transform.position, target.position);
        if (distance > npc.DetectionRange)
        {
            npc.StateMachine.ChangeState(new IdleState(npc, 5f));
        }
    }

    public void Exit()
    {
        Debug.Log("NPC прекратил погоню");
        npc.SetAnimation("Idle");
    }
}
