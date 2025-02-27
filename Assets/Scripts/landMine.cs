using UnityEngine;

public class landMine : MonoBehaviour, IDamage
{

    [SerializeField] int damage;
    [SerializeField] GameObject explosionEffect;


    bool steppedOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        steppedOn = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            steppedOn = true;
            GameManager.instance.playerScript.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Projectile"))
        {
            Explode();
        }
    }
    void Explode()
    {
        // Destroy the mine
        Destroy(gameObject);
    }

    public void takeDamage(int amount)
    {
        Explode();
    }
}
