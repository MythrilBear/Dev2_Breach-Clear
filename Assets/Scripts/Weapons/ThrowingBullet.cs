using System.Collections;
using UnityEngine;

public class ThrowingBullet : MonoBehaviour
{
    [SerializeField] private float maxAmmo;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private float throwForce;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float soundRadius;
    [SerializeField] AlertSystem alertSystemSoldier;
    [SerializeField] AlertSystem alertSystemHeavy;
    [SerializeField] AlertSystem alertSystemSniper;


    private float currentAmmo;
    private Camera mainCamera;

    void Start()
    {
        currentAmmo = maxAmmo;
        mainCamera = Camera.main;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && currentAmmo > 0)
        {
            ThrowBullet();
        }
    }

    void ThrowBullet()
    {
        // Instantiate the bullet at the throw position
        GameObject bullet = Instantiate(bulletPrefab, throwPosition.position, throwPosition.rotation);

        // Apply force to the bullet to simulate throwing
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Ensure gravity affects the bullet
            rb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);
        }

        // Play the audio clip with a delay
        StartCoroutine(PlayAudioWithDelay(1f)); // Adjust the delay as needed

        // Decrease the current ammo count
        currentAmmo--;
    }

    IEnumerator PlayAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(hitSound);
        yield return null;

        // Notify enemies of the sound
        NotifyEnemies();
    }

    void NotifyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, soundRadius);
        foreach (var hitCollider in hitColliders)
        {
            enemyAI enemy = hitCollider.GetComponent<enemyAI>();
            if (enemy != null)
            {
                enemy.investigate(transform.position, soundRadius);

                // Trigger low alert in the alert system
                if (alertSystemSoldier != null)
                {
                    alertSystemSoldier.TriggerLowAlert();
                }
                if (alertSystemHeavy != null)
                {
                    alertSystemHeavy.TriggerLowAlert();
                }
                if (alertSystemSniper != null)
                {
                    alertSystemSniper.TriggerLowAlert();
                }
            }
        }
    }
}
