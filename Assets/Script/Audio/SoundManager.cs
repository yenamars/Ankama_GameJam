using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundToPlay
{
    public AudioClip audioClip;
    public float volume;
    public Vector2 pitch;
}

public class SoundManager : MonoBehaviour 
{
    [SerializeField] private AudioSource adSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicsClips;

    public static SoundManager instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Instantiate<SoundManager>(Resources.Load<SoundManager>("SoundManager"));
            }

            return s_instance;
        }
    }

    private static SoundManager s_instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(SoundToPlay soundToPlay)
    {
        adSource.clip = soundToPlay.audioClip;
        adSource.volume = soundToPlay.volume;
        adSource.pitch = Random.Range(soundToPlay.pitch.x, soundToPlay.pitch.y);
        adSource.Play();
    }

    public void PlayMusic(int musicIndex)
    {
        musicSource.clip = musicsClips[musicIndex];
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
