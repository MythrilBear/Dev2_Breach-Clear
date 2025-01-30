using UnityEngine;

public class KnifePickup : MonoBehaviour
{
    [SerializeField] private MeleeStats knifeStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IPickup pickup = other.GetComponent<IPickup>();
            if (pickup != null)
            {
                pickup.getMeleeStats(knifeStats);
                Destroy(gameObject); // Remove the knife from the scene after pickup
            }
        }
    }
}
