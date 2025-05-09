using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    [Header("Патрулирование")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Обнаружение цели")]
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 0.1f;

    public StateMachine StateMachine { get; private set; }
    public Transform[] PatrolPoints => patrolPoints;
    public Transform Target => target;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public bool isAttacking = false;
    public Animator animator;

    private void Awake()
    {
        StateMachine = new StateMachine();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StateMachine.ChangeState(new IdleState(this, 10f)); // 10 секунд бездействия
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    private void Update()
    {
        StateMachine.Update();
        float distance = Vector3.Distance(transform.position, target.position);
        if (target != null && CanSeeTarget())
        {
            

            if (distance > attackRange)
            {
                if (!(StateMachine.CurrentState is ChaseState))
                {
                    StateMachine.ChangeState(new ChaseState(this, target));
                }
            }
            else
            {
                // AttackState можно добавить при необходимости
                StateMachine.ChangeState(new AttackState(this, target));
            }
        }
    }

    public void SetAnimation(string state)
    {
        if (animator == null) return;

        animator.ResetTrigger("Attack");
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        switch (state)
        {
            case "Idle":
                break;
            case "Walk":
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                break;
            case "Run":
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                break;
            case "Attack":
                animator.SetTrigger("Attack");
                break;
        }
    }

    private bool CanSeeTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= detectionRange;
        // Можно добавить Raycast или проверку угла обзора
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
