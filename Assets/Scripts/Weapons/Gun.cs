using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    

    [SerializeField] AudioSource aud;

    public gunStats gunStats;
    public playerController playerController;
    public Transform cameraTransform;

    private float reserveAmmo = 0f;
    public float currentAmmo = 0f;
    private float fireDelay = 0f;

    private bool isReloading = false;

    [Header("ADS functionality")]

    [SerializeField] private Transform adsPosition;
    [SerializeField] private Transform hipPosition;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float adsFOV;
    private float normalFOV;
    [SerializeField] private float adsSpeed;
    private bool isAiming = false;

    [SerializeField] float adsDown;
    [SerializeField] float adsForward;

    [Header("Recoil Settings")]
    //[Range(0, 1)]
    //public float recoilPercent = 0.3f;
    

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private bool recoiling;
    public bool recovering;


    private void Start()
    {
        currentAmmo = gunStats.magazineSize;
        reserveAmmo = gunStats.reserveAmmoMax;

        GameManager.instance.updateAmmoCount(currentAmmo);
        GameManager.instance.updateReserveAmmoCount(reserveAmmo);

        playerController = transform.root.GetComponent<playerController>();
        cameraTransform = playerController.playerCamera.transform;

        normalFOV = playerCamera.fieldOfView;

        //originalPosition = transform.localPosition;
        gunStats.recoilLength = 0;
        gunStats.recoverLength = 1 / gunStats.fireRate * gunStats.recoverPercent;
    }

    public virtual void Update()
    {
        playerController.ResetRecoil(gunStats);
        if (GameManager.instance.ammoCount != currentAmmo)
        {
            GameManager.instance.updateAmmoCount(currentAmmo);
        }
        if (GameManager.instance.reserveAmmoCount != reserveAmmo)
        {
            GameManager.instance.updateReserveAmmoCount(reserveAmmo);
        }


        if (Input.GetButton("Aim"))
        {
            isAiming = true;
            originalPosition = adsPosition.localPosition;
        }
        else
        {
            isAiming = false;
            originalPosition = hipPosition.localPosition;
        }

        //ADS Mechanic
        Vector3 targetLocalPosition = hipPosition.localPosition;
        if (isAiming)
        {
            GameManager.instance.recticle.SetActive(false);
            targetLocalPosition = adsPosition.localPosition + Vector3.forward * adsForward + Vector3.down * adsDown;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, adsFOV, adsSpeed * Time.deltaTime);
        }
        else
        {
            GameManager.instance.recticle.SetActive(true);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, adsSpeed * Time.deltaTime);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * adsSpeed);


        
        if (recoiling)
        {
            Recoil();
        }
        if (recovering)
        {
            Recover();
        }
    }

    public void tryReload()
    {
        if (!isReloading && currentAmmo < gunStats.magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        //Debug.Log(gunStats.gunName + " is reloading...");
        aud.PlayOneShot(gunStats.reloadSound[Random.Range(0, gunStats.reloadSound.Length)], gunStats.reloadSoundVol);

        yield return new WaitForSeconds(gunStats.reloadTime);

        if (reserveAmmo >= gunStats.magazineSize)
        {
            currentAmmo = gunStats.magazineSize;
            reserveAmmo -= gunStats.magazineSize;
        }
        else
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }

        isReloading = false;
        //Debug.Log(gunStats.gunName + " is reloaded.");
        GameManager.instance.updateAmmoCount(currentAmmo);
        GameManager.instance.updateReserveAmmoCount(reserveAmmo);
    }

    public void TryShoot()
    {
        if (isReloading || GameManager.instance.isPaused)
        {
            return;
        }
        else if (currentAmmo <= 0)
        {
            if (Time.time >= fireDelay)
            {
                aud.PlayOneShot(gunStats.gunEmptySound[Random.Range(0, gunStats.gunEmptySound.Length)], gunStats.gunEmptySoundVol);
                fireDelay = Time.time + (1 / gunStats.fireRate);
            }
            
            return;
        }

        if (Time.time >= fireDelay)
        {
            fireDelay = Time.time + (1 / gunStats.fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        recoiling = true;
        recovering = false;

        currentAmmo--;
        GameManager.instance.updateAmmoCount(currentAmmo);

        // recoil
        playerController.ApplyRecoil(gunStats);
        // muzzleFlash
        StartCoroutine(flashMuzzleFire());
        // shootSound

        //Debug.Log(gunStats.gunName + " shot! Bullets left: " + currentAmmo);
        aud.PlayOneShot(gunStats.shootSound[Random.Range(0, gunStats.shootSound.Length)], gunStats.shootSoundVol);
        Shoot();
    }

    IEnumerator flashMuzzleFire()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public abstract void Shoot();

    void Recoil()
    {
        Vector3 finalPosition = new Vector3
            (originalPosition.x, originalPosition.y + gunStats.recoilUp, originalPosition.z - gunStats.recoilBack);

        transform.localPosition = Vector3.SmoothDamp
            (transform.localPosition, finalPosition, ref recoilVelocity, gunStats.recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recover()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition = Vector3.SmoothDamp
            (transform.localPosition, finalPosition, ref recoilVelocity, gunStats.recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }

}
