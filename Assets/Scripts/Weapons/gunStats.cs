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
    [Space]
    [Range(0, 2)]
    public float recoverPercent = 0.2f;

    public float recoilUp = 0.02f;
    public float recoilBack = 0.08f;
    public float recoilLength;
    public float recoverLength;

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
