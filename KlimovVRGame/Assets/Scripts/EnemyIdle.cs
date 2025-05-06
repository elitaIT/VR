using UnityEngine;

[CreateAssetMenu(menuName = "NPC States/Idle")]
public class IdleState : NPCState
{
    private float stateTime;
    private int currentPatrolIndex;

    public override void EnterState(VRShooterNPC npc)
    {
        Debug.Log($"{npc.name}: Entering Idle State");
        npc.Agent.speed = npc.patrolSpeed;
        stateTime = 0f;
        currentPatrolIndex = 0;
    }

    public override void UpdateState(VRShooterNPC npc)
    {
        stateTime += Time.deltaTime;

        // Проверка на переход в состояние атаки
        if (Vector3.Distance(npc.transform.position, npc.player.position) <= npc.sightRange)
        {
            npc.ChangeState(ScriptableObject.CreateInstance<AttackState>());
            return;
        }

        // Логика патрулирования
        if (npc.patrolPoints.Length > 0)
        {
            HandlePatrol(npc);
        }
        else
        {
            HandleLookAround(npc);
        }
    }

    private void HandlePatrol(VRShooterNPC npc)
    {
        if (npc.Agent.remainingDistance <= npc.Agent.stoppingDistance)
        {
            if (stateTime > Random.Range(3f, 8f))
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % npc.patrolPoints.Length;
                npc.Agent.SetDestination(npc.patrolPoints[currentPatrolIndex].position);
                stateTime = 0f;
                Debug.Log($"{npc.name}: Moving to next patrol point");
            }
            else
            {
                LookAtRandomDirection(npc);
            }
        }
    }

    private void HandleLookAround(VRShooterNPC npc)
    {
        if (stateTime > Random.Range(3f, 8f))
        {
            LookAtRandomDirection(npc);

            if (stateTime > Random.Range(8f, 15f))
            {
                stateTime = 0f;
            }
        }
    }

    private void LookAtRandomDirection(VRShooterNPC npc)
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(randomDirection);
        npc.transform.rotation = Quaternion.Slerp(
            npc.transform.rotation,
            lookRotation,
            Time.deltaTime * npc.rotationSpeed);
    }

    public override void ExitState(VRShooterNPC npc)
    {
        Debug.Log($"{npc.name}: Exiting Idle State");
    }
}