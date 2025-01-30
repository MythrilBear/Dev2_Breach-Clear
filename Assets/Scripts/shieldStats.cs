using UnityEngine;

[CreateAssetMenu]

public class shieldStats : ScriptableObject
{
    public GameObject model;
    public int shieldHealth;
    public float rechargeRate;
    public float rechargeDelay;
    public bool isBreakable;
 
}
