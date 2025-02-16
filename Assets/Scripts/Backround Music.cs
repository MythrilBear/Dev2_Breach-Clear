using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField][Range(0f, 1f)] float volume;

    private AudioSource audioSource;

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        
        audioSource.clip = music;
        audioSource.volume = volume; 
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();
    }
}
