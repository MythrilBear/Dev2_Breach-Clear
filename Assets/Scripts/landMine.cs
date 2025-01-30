using UnityEngine;

public class landMine : MonoBehaviour
{

    [SerializeField] int damage;


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
    }
}
