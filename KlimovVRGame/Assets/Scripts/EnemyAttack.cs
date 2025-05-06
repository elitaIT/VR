using UnityEngine;

[CreateAssetMenu(menuName = "NPC States/Attack")]
public class AttackState : NPCState
{
    private float stateTime;
    private bool isTakingCover;
    private Transform currentCoverPoint;

    public override void EnterState(VRShooterNPC npc)
    {
        Debug.Log($"{npc.name}: Entering Attack State");
        npc.Agent.speed = npc.chaseSpeed;
        stateTime = 0f;
        isTakingCover = false;
    }

    public override void UpdateState(VRShooterNPC npc)
    {
        float distanceToPlayer = Vector3.Distance(npc.transform.position, npc.player.position);

        // Возврат в состояние покоя
        if (distanceToPlayer > npc.sightRange * 1.2f)
        {
            npc.ChangeState(ScriptableObject.CreateInstance<IdleState>());
            return;
        }

        // Логика атаки
        if (distanceToPlayer <= npc.attackRange && npc.HasLineOfSightToPlayer())
        {
            if (npc.coverPoints.Length > 0 && distanceToPlayer > 5f)
            {
                HandleCoverBehavior(npc);
            }
            else
            {
                HandleChaseBehavior(npc);
            }
        }
        else
        {
            HandleChaseBehavior(npc);
        }

        FacePlayer(npc);
    }

    private void HandleCoverBehavior(VRShooterNPC npc)
    {
        if (!isTakingCover || currentCoverPoint == null ||
            Vector3.Distance(npc.transform.position, currentCoverPoint.position) < 1f)
        {
            currentCoverPoint = npc.GetBestCoverPoint();
            if (currentCoverPoint != null)
            {
                npc.Agent.SetDestination(currentCoverPoint.position);
                isTakingCover = true;
                Debug.Log($"{npc.name}: Moving to cover at {currentCoverPoint.position}");
            }
        }
        else if (Vector3.Distance(npc.transform.position, currentCoverPoint.position) <= 1f)
        {
            if (npc.HasLineOfSightToPlayer())
            {
                Debug.Log($"{npc.name}: Attacking player from cover");
            }
            else
            {
                Debug.Log($"{npc.name}: No line of sight from cover");
                if (stateTime > 5f)
                {
                    isTakingCover = false;
                    stateTime = 0f;
                }
            }
        }
    }

    private void HandleChaseBehavior(VRShooterNPC npc)
    {
        isTakingCover = false;
        npc.Agent.SetDestination(npc.player.position);

        if (npc.HasLineOfSightToPlayer() &&
            Vector3.Distance(npc.transform.position, npc.player.position) <= npc.attackRange)
        {
            Debug.Log($"{npc.name}: Attacking while chasing");
        }
    }

    private void FacePlayer(VRShooterNPC npc)
    {
        Vector3 directionToPlayer = (npc.player.position - npc.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        npc.transform.rotation = Quaternion.Slerp(
            npc.transform.rotation,
            lookRotation,
            Time.deltaTime * npc.rotationSpeed);
    }

    public override void ExitState(VRShooterNPC npc)
    {
        Debug.Log($"{npc.name}: Exiting Attack State");
    }
}