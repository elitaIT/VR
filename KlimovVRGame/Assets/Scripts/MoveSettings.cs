using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MoveSettings : MonoBehaviour
{
    [Header("Movement Providers")]
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportProvider;
    public DynamicMoveProvider moveProvider;

    private const string MovementPrefKey = "MovementMode"; // 0 = Teleport, 1 = Move

    void Start()
    {
        int mode = PlayerPrefs.GetInt(MovementPrefKey, 0);
        ApplyMovementMode(mode);
    }

    public void OnMovementModeChanged(int modeIndex)
    {
        ApplyMovementMode(modeIndex);
        PlayerPrefs.SetInt(MovementPrefKey, modeIndex);
    }

    private void ApplyMovementMode(int modeIndex)
    {
        if (teleportProvider != null)
            teleportProvider.enabled = (modeIndex == 0);

        if (moveProvider != null)
            moveProvider.enabled = (modeIndex == 1);
    }
}
