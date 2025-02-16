using UnityEngine;
using UnityEngine.Audio;

public class AlertSystem : MonoBehaviour
{
    [Header("----- High Alert -----")]
    [SerializeField] float highAlertIntensity;
    [SerializeField] AudioClip[] highAlerts;

    [Header("----- Low Alert -----")]
    [SerializeField] float lowAlertIntensity;
    [SerializeField] AudioClip[] lowAlerts;

    [Header("----- No Alert -----")]
    [SerializeField] float noAlertsIntensity;
    [SerializeField] AudioClip[] noAlert;

    [SerializeField] enemyAI enemy;
    AudioSource audioSource;

    void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponent<enemyAI>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TriggerHighAlert()
    {
        if (enemy != null)
        {
            enemy.investigate(transform.position, highAlertIntensity);
        }

    }

    public void TriggerLowAlert()
    {
        if (enemy != null)
        {
            enemy.investigate(transform.position, lowAlertIntensity);
        }

    }

    public void TriggerNoAlert()
    {
        if (enemy != null)
        {
            enemy.investigate(transform.position, noAlertsIntensity);
        }

    }

    public void enemyHeardSound(Vector3 soundPos, float radius)
    {
        if (enemy != null)
        {
            enemy.investigate(soundPos, radius);
        }
    }

    
}