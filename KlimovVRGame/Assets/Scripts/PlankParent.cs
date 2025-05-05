using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlankParent : MonoBehaviour
{
    public Rigidbody rb; // Ссылка на Rigidbody прикрепленного объекта
    public Collider axeCollider; // Ссылка на коллайдер топора
    public float detachmentForce = 1f; // Сила, с которой объект отлетает

    private bool isAttached = true; // Флаг, указывающий, прикреплен ли объект

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что столкнулись с топором и объект еще прикреплен
        if (isAttached && collision.collider == axeCollider)
        {
            DetachObject();
        }
    }

    private void DetachObject()
    {
        isAttached = false;

        // Активируем физику, если она была отключена
        if (rb != null)
        {
            rb.isKinematic = false;

            // Добавляем небольшой импульс в направлении от топора
            Vector3 forceDirection = (transform.position - axeCollider.transform.position).normalized;
            rb.AddForce(forceDirection * detachmentForce, ForceMode.Impulse);
        }

        // Отключаем этот скрипт, так как объект уже откреплен
        enabled = false;
    }
}