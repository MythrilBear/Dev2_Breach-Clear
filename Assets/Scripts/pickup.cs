using UnityEngine;

public class pickup : MonoBehaviour
{

    [SerializeField] shieldStats shield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();
        if(pick != null)
        {
            pick.getShieldStats(shield);
            Destroy(gameObject);
        }
    }
}
