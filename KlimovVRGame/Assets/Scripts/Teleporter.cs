using UnityEngine;

public class CharacterControllerTeleport : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private Transform xrOrigin; // XR Origin объект
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform targetPoint; // Целевая точка
    [Header("Настройки смещения")]
    [SerializeField] private bool applyHeightCorrection = true; // Коррекция высоты

    private Vector3 _initialControllerCenter;
    private float _initialControllerHeight;

    private void Start()
    {
        // Сохраняем первоначальные параметры Character Controller
        _initialControllerCenter = characterController.center;
        _initialControllerHeight = characterController.height;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportTrigger"))
        {
            PerformCharacterControllerTeleport();
        }
    }

    private void PerformCharacterControllerTeleport()
    {
        // Отключаем Character Controller для телепортации
        characterController.enabled = false;

        // Рассчитываем смещение камеры относительно центра Character Controller
        Vector3 cameraOffset = Camera.main.transform.position -
                              (xrOrigin.position + characterController.center);

        // Применяем новую позицию
        xrOrigin.position = targetPoint.position - cameraOffset;

        // Корректируем высоту при необходимости
        if (applyHeightCorrection)
        {
            AdjustControllerHeight();
        }

        // Включаем Character Controller обратно
        characterController.enabled = true;
    }

    private void AdjustControllerHeight()
    {
        // Рассчитываем реальную высоту игрока
        float playerHeight = Camera.main.transform.localPosition.y;

        // Корректируем параметры Character Controller
        characterController.height = playerHeight;
        characterController.center = new Vector3(
            _initialControllerCenter.x,
            playerHeight * 0.5f + characterController.skinWidth,
            _initialControllerCenter.z
        );
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (targetPoint != null && characterController != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(targetPoint.position + characterController.center, 
                              new Vector3(characterController.radius*2, 
                                       characterController.height, 
                                       characterController.radius*2));
        }
    }
#endif
}