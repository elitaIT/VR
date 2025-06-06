using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown movementDropdown;
    public TMP_Dropdown turnDropdown;

    public MoveSettings moveSettings;
    public RotateSettings rotateSettings;

    private const string MoveKey = "MoveMode";
    private const string TurnKey = "TurnMode";

    void Start()
    {
        // Получаем сохранённые значения или значения по умолчанию
        int savedMoveMode = PlayerPrefs.GetInt(MoveKey, 0);
        int savedTurnMode = PlayerPrefs.GetInt(TurnKey, 1); // Snap по умолчанию

        // Устанавливаем индексы в UI
        if (movementDropdown != null)
        {
            movementDropdown.value = savedMoveMode;
            movementDropdown.RefreshShownValue();
            movementDropdown.onValueChanged.AddListener(OnMovementChanged);
        }

        if (turnDropdown != null)
        {
            turnDropdown.value = savedTurnMode;
            turnDropdown.RefreshShownValue();
            turnDropdown.onValueChanged.AddListener(OnTurnChanged);
        }

        // Применяем настройки
        moveSettings?.SetMovementModeByIndex(savedMoveMode);
        rotateSettings?.SetTurnModeByIndex(savedTurnMode);
    }

    void OnMovementChanged(int index)
    {
        moveSettings?.SetMovementModeByIndex(index);
        PlayerPrefs.SetInt(MoveKey, index);
    }

    void OnTurnChanged(int index)
    {
        rotateSettings?.SetTurnModeByIndex(index);
        PlayerPrefs.SetInt(TurnKey, index);
    }
}
