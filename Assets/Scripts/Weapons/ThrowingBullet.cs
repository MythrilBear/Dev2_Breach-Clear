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

    private float currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
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
            rb.AddForce(throwPosition.forward * throwForce, ForceMode.Impulse);
        }

        // Play the audio clip with a delay
        StartCoroutine(PlayAudioWithDelay(0.5f)); // Adjust the delay as needed

        // Decrease the current ammo count
        currentAmmo--;
    }

    IEnumerator PlayAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(hitSound);
    }
}
