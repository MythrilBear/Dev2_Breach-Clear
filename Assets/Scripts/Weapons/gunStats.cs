using UnityEngine;

[CreateAssetMenu (fileName = "NewGunData", menuName = "Gun/GunData")]
public class gunStats : ScriptableObject
{
    public string gunName;

    public LayerMask targetLayerMask;

    [Header("Fire Config")]
    public float shootingRange;
    public float fireRate;

    [Header("Reload Config")]
    public float magazineSize;
    public float reserveAmmoMax;
    public float reloadTime;

    [Header("Recoil Settings")]
    public float recoilAmount;
    public Vector2 maxRecoil;
    public float recoilSpeed;
    public float resetRecoilSpeed;

    //public GameObject model;
    public int shootDamage;
    //public int shootDist;
    //public float shootRate;
    //public int ammoCur, ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    public float shootSoundVol;
    public AudioClip[] reloadSound;
    public float reloadSoundVol;
    public AudioClip[] gunEmptySound;
    public float gunEmptySoundVol;
}
