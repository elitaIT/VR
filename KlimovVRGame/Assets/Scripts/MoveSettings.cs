using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MovementSettings : MonoBehaviour
{
    public GameObject moveProviderObject;
    private DynamicMoveProvider moveProvider;

    public GameObject teleportationProviderObject;
    public GameObject teleportRayObject;

    public GameObject continuousTurnProviderObject;
    private ContinuousTurnProviderBase continuousTurnProvider;

    public GameObject snapTurnProviderObject;
    private SnapTurnProviderBase snapTurnProvider;

    public TMP_Dropdown movementDropdown;
    public TMP_Dropdown turnDropdown;

    private void Awake()
    {
        if (moveProviderObject != null)
            moveProvider = moveProviderObject.GetComponent<DynamicMoveProvider>();

        if (continuousTurnProviderObject != null)
            continuousTurnProvider = continuousTurnProviderObject.GetComponent<ContinuousTurnProviderBase>();

        if (snapTurnProviderObject != null)
            snapTurnProvider = snapTurnProviderObject.GetComponent<SnapTurnProviderBase>();
    }

    private void Start()
    {
        ApplySavedSettings();
    }

    public void OnMovementModeChanged(int index)
    {
        bool useTeleport = index == 1;

        if (moveProvider != null)
            moveProvider.enabled = !useTeleport;

        if (teleportationProviderObject != null)
            teleportationProviderObject.SetActive(useTeleport);

        if (teleportRayObject != null)
            teleportRayObject.SetActive(useTeleport);

        PlayerPrefs.SetInt("MovementMode", index);
    }

    public void OnTurnModeChanged(int index)
    {
        if (continuousTurnProvider != null)
            continuousTurnProvider.enabled = index == 1;

        if (snapTurnProvider != null)
            snapTurnProvider.enabled = index == 2;

        PlayerPrefs.SetInt("TurnMode", index);
        Debug.Log("Turn mode changed to: " + index);
    }

    private void ApplySavedSettings()
    {
        int moveMode = PlayerPrefs.GetInt("MovementMode", 0);
        int turnMode = PlayerPrefs.GetInt("TurnMode", 1);

        if (movementDropdown != null)
            movementDropdown.SetValueWithoutNotify(moveMode);

        if (turnDropdown != null)
            turnDropdown.SetValueWithoutNotify(turnMode);

        OnMovementModeChanged(moveMode);
        OnTurnModeChanged(turnMode);
    }
}
