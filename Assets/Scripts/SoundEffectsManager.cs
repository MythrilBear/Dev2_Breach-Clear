using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private static SoundEffectsManager instance;
    private AudioSource[] audioSources;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSources = GetComponents<AudioSource>();
    }

    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }
}
