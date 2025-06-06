using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCController))]
public class NPCShooter : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float weaponRange = 10f;        // Дальность стрельбы
    [SerializeField] private float weaponDamage = 15f;       // Урон за выстрел
    [SerializeField] private float fireRate = 1f;            // Скорострельность (выстрелов в секунду)
    [SerializeField] private GameObject projectilePrefab;    // Префаб снаряда
    [SerializeField] private Transform firePoint;           // Точка вылета снаряда

    [Header("Detection Settings")]
    [SerializeField] private float detectionAngle = 90f;     // Угол обзора для обнаружения
    [SerializeField] private LayerMask targetMask;           // Маска целей
    [SerializeField] private LayerMask obstacleMask;         // Маска препятствий

    private NPCController npcController;
    private float nextFireTime;
    private Transform currentTarget;

    private void Awake()
    {
        npcController = GetComponent<NPCController>();
    }

    private void Update()
    {
        if (npcController.Target != null)
        {
            currentTarget = npcController.Target;

            if (CanShootTarget() && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private bool CanShootTarget()
    {
        if (currentTarget == null) return false;

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        // Проверка дистанции
        if (distanceToTarget > weaponRange) return false;

        // Проверка угла обзора
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget > detectionAngle / 2f) return false;

        // Проверка на препятствия
        if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
        {
            return false;
        }

        return true;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || currentTarget == null) return;

        // Создаем снаряд
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(currentTarget.position - firePoint.position)
        );

        // Настраиваем снаряд
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDamage(weaponDamage);
            projectileScript.SetRange(weaponRange);
        }

        // Активируем анимацию атаки
        npcController.SetAnimation("Attack");
    }

    // Методы для настройки параметров оружия
    public void SetWeaponRange(float newRange)
    {
        weaponRange = newRange;
    }

    public void SetWeaponDamage(float newDamage)
    {
        weaponDamage = newDamage;
    }

    public void SetFireRate(float newRate)
    {
        fireRate = newRate;
    }

    // Для визуализации в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponRange);

        // Рисуем угол обзора
        Vector3 leftDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftDirection * weaponRange);
        Gizmos.DrawRay(transform.position, rightDirection * weaponRange);
    }
}