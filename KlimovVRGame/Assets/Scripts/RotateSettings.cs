using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public enum TurnMode
{
    Off,
    Snap,
    Smooth
}

public class RotateSettings : MonoBehaviour
{
    public SnapTurnProvider snapTurnProvider;
    public ContinuousTurnProvider continuousTurnProvider;
    public ControllerInputActionManager inputActionManager;

    private const string TurnKey = "TurnMode";

    void Start()
    {
        TurnMode savedMode = (TurnMode)PlayerPrefs.GetInt(TurnKey, (int)TurnMode.Snap);
        ApplyTurnMode(savedMode);
    }

    public void ApplySavedTurnMode()
    {
        var saved = (TurnMode)PlayerPrefs.GetInt("TurnMode", (int)TurnMode.Snap);
        SetTurnModeByIndex((int)saved);
    }

    public void SetTurnModeByIndex(int index)
    {
        TurnMode selected = (TurnMode)index;
        ApplyTurnMode(selected);
    }

    private void ApplyTurnMode(TurnMode mode)
    {
        if (snapTurnProvider)
            snapTurnProvider.enabled = (mode == TurnMode.Snap);

        if (continuousTurnProvider)
            continuousTurnProvider.enabled = (mode == TurnMode.Smooth);

        if (inputActionManager)
            inputActionManager.smoothTurnEnabled = (mode == TurnMode.Smooth);

        if (mode == TurnMode.Off)
        {
            if (snapTurnProvider) snapTurnProvider.enabled = false;
            if (continuousTurnProvider) continuousTurnProvider.enabled = false;

            if (inputActionManager)
                inputActionManager.smoothTurnEnabled = false;
        }

        PlayerPrefs.SetInt(TurnKey, (int)mode);
    }
}
