using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Tooltip("Ссылка на игрока (обычно Camera Rig или XR Origin)")]
    public Transform player;

    [Tooltip("Максимальная дистанция обнаружения игрока")]
    public float detectionRange = 10f;

    [Tooltip("Если включено, враг будет учитывать направление взгляда (угол)")]
    public bool useFieldOfView = true;

    [Tooltip("Угол обзора врага в градусах (если useFieldOfView включен)")]
    public float fieldOfViewAngle = 110f;

    void Update()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Проверка по дистанции
        if (distanceToPlayer <= detectionRange)
        {
            // Проверка по углу обзора, если включено
            if (useFieldOfView)
            {
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                if (angle <= fieldOfViewAngle * 0.5f)
                {
                    Debug.Log("Игрок обнаружен по углу и дистанции");
                    // Здесь можно вызвать поведение врага
                }
            }
            else
            {
                Debug.Log("Игрок обнаружен по дистанции");
                // Здесь можно вызвать поведение врага
            }
        }
    }

    // Для визуализации в редакторе
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (useFieldOfView)
        {
            Vector3 leftRayDirection = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
            Vector3 rightRayDirection = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, leftRayDirection * detectionRange);
            Gizmos.DrawRay(transform.position, rightRayDirection * detectionRange);
        }
    }
}
