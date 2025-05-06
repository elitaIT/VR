using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VRShooterNPC : MonoBehaviour
{
    public float sightRange = 20f;
    public float attackRange = 15f;
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 4f;
    public float rotationSpeed = 5f;

    public Transform player;
    public Transform[] patrolPoints;
    public Transform[] coverPoints;

    [HideInInspector] public NavMeshAgent Agent { get; private set; }
    private NPCState currentState;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        InitializeStates();
    }

    private void InitializeStates()
    {
        // Создаем начальное состояние
        ChangeState(new IdleState());
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    public void ChangeState(NPCState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

    public bool HasLineOfSightToPlayer()
    {
        if (player == null) return false;

        Vector3 direction = player.position - transform.position;
        return !Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            direction,
            direction.magnitude);
    }

    public Transform GetBestCoverPoint()
    {
        if (coverPoints.Length == 0) return null;

        Transform bestCover = coverPoints[0];
        float bestScore = float.MinValue;

        foreach (Transform cover in coverPoints)
        {
            float distanceToPlayer = Vector3.Distance(cover.position, player.position);
            if (distanceToPlayer < 5f) continue;

            Vector3 coverToPlayer = player.position - cover.position;
            bool hasLOS = Physics.Raycast(cover.position, coverToPlayer.normalized, coverToPlayer.magnitude);

            float score = hasLOS ? 0 : 1;
            score += Random.Range(0f, 0.2f);

            if (score > bestScore)
            {
                bestScore = score;
                bestCover = cover;
            }
        }

        return bestCover;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}