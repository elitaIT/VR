using UnityEngine;

public class CharacterControllerTeleport : MonoBehaviour
{
    [Header("�������� ���������")]
    [SerializeField] private Transform xrOrigin; // XR Origin ������
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform targetPoint; // ������� �����
    [Header("��������� ��������")]
    [SerializeField] private bool applyHeightCorrection = true; // ��������� ������

    private Vector3 _initialControllerCenter;
    private float _initialControllerHeight;

    private void Start()
    {
        // ��������� �������������� ��������� Character Controller
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
        // ��������� Character Controller ��� ������������
        characterController.enabled = false;

        // ������������ �������� ������ ������������ ������ Character Controller
        Vector3 cameraOffset = Camera.main.transform.position -
                              (xrOrigin.position + characterController.center);

        // ��������� ����� �������
        xrOrigin.position = targetPoint.position - cameraOffset;

        // ������������ ������ ��� �������������
        if (applyHeightCorrection)
        {
            AdjustControllerHeight();
        }

        // �������� Character Controller �������
        characterController.enabled = true;
    }

    private void AdjustControllerHeight()
    {
        // ������������ �������� ������ ������
        float playerHeight = Camera.main.transform.localPosition.y;

        // ������������ ��������� Character Controller
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