using UnityEngine;

public class ThrowingBullet : MonoBehaviour
{
    [SerializeField] private int maxAmmo;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private float throwForce;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource audioSource;

    private int currentAmmo;

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
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, throwPosition.position, throwPosition.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(throwPosition.forward * throwForce, ForceMode.VelocityChange);

        bullet.AddComponent<BulletCollision>().Initialize(hitSound, audioSource);
    }
}

public class BulletCollision : MonoBehaviour
{
    private AudioClip hitSound;
    private AudioSource audioSource;

    public void Initialize(AudioClip hitSound, AudioSource audioSource)
    {
        this.hitSound = hitSound;
        this.audioSource = audioSource;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
