using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 20f;       // Скорость полета
    [SerializeField] private float lifetime = 3f;    // Макс время жизни (сек)

    [Header("Damage Settings")]
    [SerializeField] private GameObject hitEffect;   // Эффект при попадании

    private float damage;           // Урон (устанавливается извне)
    private float maxRange;         // Макс дистанция (устанавливается извне)
    private Vector3 startPosition;  // Точка старта
    private float currentDistance;  // Пройденная дистанция

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime); // Автоуничтожение через заданное время
    }

    private void Update()
    {
        // Движение вперед с постоянной скоростью
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Расчет пройденной дистанции
        currentDistance = Vector3.Distance(startPosition, transform.position);

        // Уничтожение при превышении дистанции
        if (currentDistance >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Игнорируем столкновения с самим собой и другими снарядами
        if (other.gameObject == transform.root.gameObject ||
            other.GetComponent<Projectile>() != null)
            return;

        // Применяем урон если объект может его получать
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

            // Дополнительный эффект (например, отталкивание)
            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.AddForce(
                    transform.forward * 5f,
                    ForceMode.Impulse
                );
            }
        }

        // Создаем эффект попадания
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Уничтожаем снаряд после попадания
    }

    // Методы для настройки параметров
    public void SetDamage(float newDamage) => damage = newDamage;
    public void SetRange(float newRange) => maxRange = newRange;
}