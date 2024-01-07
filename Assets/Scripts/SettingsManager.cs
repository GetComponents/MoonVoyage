using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    float baseMouseSpeed = 50;
    [SerializeField]
    private Slider MouseSpeedSlider;
    [SerializeField]
    private Slider masterSlider, effectsSlider, musicSlider;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("MouseSpeed") < 0.01f)
        {
            PlayerPrefs.SetFloat("MouseSpeed", 0.5f * baseMouseSpeed);
        }
    }

    private void OnEnable()
    {
        OpenSettings();
    }

    public void OpenSettings()
    {
        MouseSpeedSlider.value = PlayerPrefs.GetFloat("MouseSpeed") / baseMouseSpeed;
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void DefaultPreferences()
    {
        PlayerPrefs.SetFloat("MouseSpeed", PlayerPrefs.GetFloat("MouseSpeed", 0.5f * baseMouseSpeed));
        PlayerPrefs.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 1));
        PlayerPrefs.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("EffectsVolume", 1));
        PlayerPrefs.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 1));
    }

    public void ChangeSound(int index)
    {
        switch (index)
        {
            case 0:
                SoundManager.Instance.ChangeMasterVolume(masterSlider.value);
                break;
            case 1:
                SoundManager.Instance.ChangeSoundEffectVolume(effectsSlider.value);
                break;
            case 2:
                SoundManager.Instance.ChangeMusicVolume(musicSlider.value);
                break;
            default:
                break;
        }
    }

    public void ChangeMouseSpeed()
    {
        float mouseSpeed = MouseSpeedSlider.value * baseMouseSpeed;
        if (GloopMain.Instance != null)
        {
            GloopMain.Instance.lookSensitivity = mouseSpeed;
        }
        PlayerPrefs.SetFloat("MouseSpeed", mouseSpeed);
    }
}
