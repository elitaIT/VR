using UnityEngine;

public class TeleportationTrigger : MonoBehaviour
{
    public Transform teleportDestination;  // Место назначения для телепортации
    private CharacterController characterController;  // Ссылка на CharacterController игрока

    private void Start()
    {
        // Получаем ссылку на CharacterController игрока
        characterController = FindObjectOfType<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (other.CompareTag("Player"))  
        {
            Teleport(other.transform);  // Телепортируем игрока
        }
    }

    private void Teleport(Transform player)
    {
        if (teleportDestination != null && characterController != null)
        {
            // Получаем положение и ротацию точки назначения
            Vector3 destinationPosition = teleportDestination.position;
            Quaternion destinationRotation = teleportDestination.rotation;

            // Снимаем смещение, которое вызывает CharacterController
            Vector3 offset = characterController.center;

            // Рассчитываем корректную позицию с учетом смещения
            Vector3 correctedPosition = destinationPosition - offset;

            // Телепортируем игрока в исправленную позицию
            player.position = correctedPosition;
            player.rotation = destinationRotation;  // Если нужно, сохраняем ротацию
        }
    }
}
