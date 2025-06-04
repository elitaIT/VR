using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderInfo : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;

    public TMP_Text masterVolumeText;
    public TMP_Text musicVolumeText;
    public TMP_Text effectsVolumeText;

    private void Start()
    {
        // »нициализаци€ значений из PlayerPrefs или по умолчанию
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1f);

        // ќбновл€ем текст р€дом слайдера при старте
        UpdateMasterText(masterVolumeSlider.value);
        UpdateMusicText(musicVolumeSlider.value);
        UpdateEffectsText(effectsVolumeSlider.value);

        // ƒобавл€ем слушатели на изменение слайдеров
        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterText);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicText);
        effectsVolumeSlider.onValueChanged.AddListener(UpdateEffectsText);
    }

    // ‘ункции обновлени€ текста р€дом с каждым слайдером
    private void UpdateMasterText(float value)
    {
        masterVolumeText.text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    private void UpdateMusicText(float value)
    {
        musicVolumeText.text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void UpdateEffectsText(float value)
    {
        effectsVolumeText.text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("EffectsVolume", value);
    }
}
