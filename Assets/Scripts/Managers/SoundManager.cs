using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public List<AudioSource> SoundEffects;

    [SerializeField]
    private AudioMixer soundMixer;
    [SerializeField]
    AudioMixerGroup effectsGroup, soundtrackGroup;

    public AudioSource Soundtrack;
    public bool PlaySoundtrack
    {
        get => playSoundtrack;
        set
        {
            if (playSoundtrack == value || Soundtrack == null)
                return;
            if (value)
            {
                Soundtrack.volume = startVolume;
            }
            else
            {
                Soundtrack.volume = 0;
            }
            playSoundtrack = value;
        }
    }
    float startVolume;
    private bool playSoundtrack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (Soundtrack != null)
            startVolume = Soundtrack.volume;
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1)) * 20);
        soundMixer.SetFloat("EffectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("EffectsVolume", 1)) * 20);
        soundMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEffect(AudioClip effect, float volume = 1)
    {
        for (int i = 0; i < SoundEffects.Count; i++)
        {
            if (!SoundEffects[i].isPlaying)
            {
                SoundEffects[i].volume = volume;
                SoundEffects[i].clip = effect;
                SoundEffects[i].Play();
                return;
            }
        }
        AudioSource tmp = gameObject.AddComponent<AudioSource>();
        tmp.outputAudioMixerGroup = effectsGroup;
        tmp.clip = effect;
        tmp.volume = volume;
        tmp.Play();
        SoundEffects.Add(tmp);
    }

    public void ChangeMasterVolume(float amount)
    {
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("MasterVolume", amount);
    }

    public void ChangeSoundEffectVolume(float amount)
    {
        soundMixer.SetFloat("EffectsVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", amount);
    }

    public void ChangeMusicVolume(float amount)
    {
        soundMixer.SetFloat("MusicVolume", Mathf.Log10(amount) * 20);
        PlayerPrefs.SetFloat("MusicVolume", amount);
    }
}
