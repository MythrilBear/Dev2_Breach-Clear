using UnityEngine;

public class TossedGrenade : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0, 1, 0);

    [Header("Explosion Settings")]
    [SerializeField] private float explosionDelay = 3f;
    //[SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage;



    private float countdown;
    private bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = explosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasExploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);

        CheckForEntities();

        // Uncomment the following line if explosion does not go away.
        //Destroy(explosionEffect, 4f);

        // play sound effect here

        Destroy(gameObject);
    }

    private void CheckForEntities()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider entity in colliders)
        {
            IDamage dmg = entity.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(explosionDamage);
            }
        }
    }
}
