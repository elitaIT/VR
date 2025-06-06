using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    void Start()
    {
        MoveSettings move = FindObjectOfType<MoveSettings>();
        if (move) move.ApplySavedMovementMode();

        RotateSettings rotate = FindObjectOfType<RotateSettings>();
        if (rotate) rotate.ApplySavedTurnMode();
    }
}
