using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource sfxObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource source = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        source.clip = clip;

        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;

        Destroy(source.gameObject, clipLength);
    }

    public void PlayRandomSFX(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        int idx = Random.Range(0, clips.Length);

        AudioSource source = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        source.clip = clips[idx];

        source.volume = volume;

        source.Play();

        float clipLength = source.clip.length;

        Destroy(source.gameObject, clipLength);
    }
}
