using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class RotateSettings : MonoBehaviour
{
    [Header("Turn Providers")]
    public SnapTurnProvider snapTurnProvider;
    public ContinuousTurnProvider continuousTurnProvider;

private const string TurnPrefKey = "TurnMode"; // 0 = Off, 1 = Snap, 2 = Smooth

    void Start()
    {
        int mode = PlayerPrefs.GetInt(TurnPrefKey, 0);
        ApplyTurnMode(mode);
    }

    public void OnTurnModeChanged(int modeIndex)
    {
        ApplyTurnMode(modeIndex);
        PlayerPrefs.SetInt(TurnPrefKey, modeIndex);
    }

    private void ApplyTurnMode(int modeIndex)
    {
        if (snapTurnProvider != null)
            snapTurnProvider.enabled = (modeIndex == 1);

        if (continuousTurnProvider != null)
            continuousTurnProvider.enabled = (modeIndex == 2);

        if (modeIndex == 0)
        {
            if (snapTurnProvider != null) snapTurnProvider.enabled = false;
            if (continuousTurnProvider != null) continuousTurnProvider.enabled = false;
        }
    }
}
