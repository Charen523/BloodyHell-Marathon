using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    public AudioClip[] bgmClip; //BuildIndex에 따라 BGM 할당.

    private AudioSource bgmAudioSource;
    private int sceneNum = -1;
    
    public bool[] isMuted = new bool[3];
    private float[] preVolumes = new float[3];

    protected override void Awake()
    {
        canDestroyOnLoad = false;
        base.Awake();

        bgmAudioSource = GetComponent<AudioSource>();
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }

        // BGM 믹서 그룹 설정
        bgmAudioSource.outputAudioMixerGroup = bgmMixerGroup;
        bgmAudioSource.loop = true;

        if (bgmClip == null || bgmClip.Length == 0)
        {
            Debug.LogError("BGM Clips not assigned in inspector.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneNum != -1 && bgmClip[sceneNum] == bgmClip[scene.buildIndex])
            return;

        sceneNum = scene.buildIndex;

        try
        {
            if (bgmClip[sceneNum] != null)
            {
                ChangeBackGroundMusic(bgmClip[sceneNum]);
            }
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log("BGM Index Error");

            if (bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Stop();
            }
        }
    }

    private void ChangeBackGroundMusic(AudioClip music)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = music;
        bgmAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        //효과음.
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void ToggleMasterMute()
    {
        isMuted[0] = !isMuted[0]; 
        
        if (isMuted[0])
        {
            audioMixer.GetFloat("MasterVolume", out preVolumes[0]);
            audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", preVolumes[0]);
        }
    }

    public void ToggleBGMMute()
    {
        isMuted[1] = !isMuted[1];

        if (isMuted[1])
        {
            audioMixer.GetFloat("BGMVolume", out preVolumes[1]);
            audioMixer.SetFloat("BGMVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("BGMVolume", preVolumes[1]);
        }
    }

    public void ToggleSFXMute()
    {
        isMuted[2] = !isMuted[2];

        if (isMuted[2])
        {
            audioMixer.GetFloat("SFXVolume", out preVolumes[2]);
            audioMixer.SetFloat("SFXVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", preVolumes[2]);
        }
    }
}