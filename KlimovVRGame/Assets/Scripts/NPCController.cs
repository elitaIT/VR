using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    [Header("Патрулирование")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Обнаружение цели")]
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange = 8f;

    public StateMachine StateMachine { get; private set; }
    public Transform[] PatrolPoints => patrolPoints;
    public Transform Target => target;

    private Animator animator;

    private void Awake()
    {
        StateMachine = new StateMachine();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StateMachine.ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        StateMachine.Update();

        if (target != null && CanSeeTarget())
        {
            float distance = Vector3.Distance(transform.position, target.position);

            // Если цель в пределах обнаружения, но вне радиуса атаки — перейти в ChaseState
            if (distance > 2f && !(StateMachine.CurrentState is ChaseState))
            {
                StateMachine.ChangeState(new ChaseState(this, target));
            }
            // Если уже рядом — перейти в атаку
            else if (distance <= 2f && !(StateMachine.CurrentState is AttackState))
            {
                StateMachine.ChangeState(new AttackState(this, target));
            }
        }
    }

    public void SetAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }

    private bool CanSeeTarget()
    {
        return Vector3.Distance(transform.position, target.position) <= detectionRange;
        // Можно заменить на Raycast или поле зрения
    }
}
