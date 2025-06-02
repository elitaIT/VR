using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Audio;

public class VRSettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText;

    [Header("Movement")]
    public ContinuousMoveProviderBase continuousMove;
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportation;
    public TMP_Dropdown movementDropdown;
    public TextMeshProUGUI movementTypeText;

    [Header("Rotation")]
    public XROrigin xrOrigin; // Перетащите сюда XR Origin из сцены
    public TMP_Dropdown rotationDropdown;
    public Slider turnSpeedSlider;
    public TextMeshProUGUI turnSpeedValueText;

    private ContinuousTurnProviderBase continuousTurn;
    private SnapTurnProviderBase snapTurn;

    private void Start()
    {
        FindTurnComponents();
        InitializeUI();
        LoadSettings();
    }

    private void FindTurnComponents()
    {
        // Автопоиск компонентов поворота
        if (xrOrigin != null)
        {
            continuousTurn = xrOrigin.GetComponentInChildren<ContinuousTurnProviderBase>();
            snapTurn = xrOrigin.GetComponentInChildren<SnapTurnProviderBase>();
        }

        if (continuousTurn == null || snapTurn == null)
            Debug.LogError("Компоненты поворота не найдены!");
    }

    private void InitializeUI()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        movementDropdown.onValueChanged.AddListener(SetMovementType);
        rotationDropdown.onValueChanged.AddListener(SetRotationType);
        turnSpeedSlider.onValueChanged.AddListener(SetTurnSpeed);
    }

    public void SetVolume(float value)
    {
        float volume = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        volumeValueText.text = $"{Mathf.RoundToInt(volume * 100)}%";
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetMovementType(int index)
    {
        continuousMove.enabled = (index == 0);
        teleportation.enabled = (index == 1);
        movementTypeText.text = movementDropdown.options[index].text;
        PlayerPrefs.SetInt("MovementType", index);
    }

    public void SetRotationType(int index)
    {
        if (continuousTurn != null) continuousTurn.enabled = (index == 1);
        if (snapTurn != null) snapTurn.enabled = (index == 0);
        PlayerPrefs.SetInt("RotationType", index);
    }

    public void SetTurnSpeed(float speed)
    {
        if (continuousTurn != null)
        {
            continuousTurn.turnSpeed = speed * 180f;
            turnSpeedValueText.text = $"{Mathf.RoundToInt(speed * 100)}%";
            PlayerPrefs.SetFloat("TurnSpeed", speed);
        }
    }

    private void LoadSettings()
    {
        // Загрузка сохранённых настроек
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
        movementDropdown.value = PlayerPrefs.GetInt("MovementType", 0);
        rotationDropdown.value = PlayerPrefs.GetInt("RotationType", 0);
        turnSpeedSlider.value = PlayerPrefs.GetFloat("TurnSpeed", 0.5f);
    }

    void OnDisable() => PlayerPrefs.Save();
}