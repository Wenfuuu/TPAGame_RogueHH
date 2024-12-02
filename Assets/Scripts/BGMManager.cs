using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource bgmSource; // The AudioSource playing BGM
    private float targetVolume = 0.5f;
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (bgmSource != null && bgmSource.volume != targetVolume)
        {
            bgmSource.volume = Mathf.MoveTowards(bgmSource.volume, targetVolume, Time.deltaTime / fadeDuration);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return; // Avoid restarting if the same clip is already playing

        bgmSource.clip = clip;
        //bgmSource.volume = 0;
        bgmSource.Play();
    }

    public void FadeOutBGM()
    {
        targetVolume = 0f;
    }

    public void FadeInBGM()
    {
        targetVolume = 0.5f;
    }
}
