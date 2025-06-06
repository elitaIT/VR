using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum MovementMode
{
    Teleport,
    Joystick
}

public class MoveSettings : MonoBehaviour
{
    public ControllerInputActionManager inputActionManager;
    public DynamicMoveProvider moveProvider;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;
    public GameObject leftTeleportRay;

    private const string MoveKey = "MoveMode";

    void Start()
    {
        var saved = (MovementMode)PlayerPrefs.GetInt(MoveKey, 0);
        SetMovementMode(saved);
    }

    public void ApplySavedMovementMode()
    {
        var saved = (MovementMode)PlayerPrefs.GetInt("MoveMode", 0);
        SetMovementMode(saved);
    }

    public void SetMovementMode(MovementMode mode)
    {
        bool isTeleport = mode == MovementMode.Teleport;

        if (moveProvider)
        {
            moveProvider.enabled = !isTeleport;
            
        }
        if (teleportationProvider)
        {
            teleportationProvider.enabled = isTeleport;
        }
        if (leftTeleportRay)
        {
            leftTeleportRay.SetActive(isTeleport);
        }
        if (inputActionManager)
        {
            inputActionManager.smoothMotionEnabled = !isTeleport; // вот так правильно
        }

        PlayerPrefs.SetInt(MoveKey, (int)mode);
    }

    // ��� Dropdown � ���������� ����� OnValueChanged(int)
    public void SetMovementModeByIndex(int index)
    {
        SetMovementMode((MovementMode)index);
    }
}
