using UnityEngine;

public class healthRecover : MonoBehaviour
{
    [SerializeField] int HPRecover;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<playerController>().recoverHealth(+HPRecover);
            Destroy(gameObject);
        }
    }
}
