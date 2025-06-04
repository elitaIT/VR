using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadVolume("Master", masterSlider);
        LoadVolume("Music", musicSlider);
        LoadVolume("SFX", sfxSlider);
    }

    public void OnMasterVolumeChanged(float value)
    {
        mixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }

    public void OnMusicVolumeChanged(float value)
    {
        mixer.SetFloat("Music", Mathf.Log10(value) * 20);
    }

    public void OnSFXVolumeChanged(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }

    private void SetVolume(string param, float value)
    {
        mixer.SetFloat(param, Mathf.Log10(value) * 20); // dB
        PlayerPrefs.SetFloat(param, value);
    }

    private void LoadVolume(string param, Slider slider)
    {
        float value = PlayerPrefs.GetFloat(param, 0.75f);
        slider.value = value;
        mixer.SetFloat(param, Mathf.Log10(value) * 20);
    }
}
