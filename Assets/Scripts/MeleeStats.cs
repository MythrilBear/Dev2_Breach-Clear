using UnityEngine;
[CreateAssetMenu]
public class MeleeStats : ScriptableObject
{
    public GameObject model;
    public int damage;
    public float attackRate;
    public AudioClip[] attackSound;
    public float attackSoundVol;
}
