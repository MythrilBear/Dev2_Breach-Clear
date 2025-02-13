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
    private float currentAmmo = 0f;
    private float fireDelay = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunStats.magazineSize;
        reserveAmmo = gunStats.reserveAmmoMax;

        GameManager.instance.updateAmmoCount(currentAmmo);

        playerController = transform.root.GetComponent<playerController>();
        cameraTransform = playerController.playerCamera.transform;
    }

    public virtual void Update()
    {
        //playerController.ResetRecoil(gunStats);
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

        Debug.Log(gunStats.gunName + " is reloading...");
        aud.PlayOneShot(gunStats.reloadSound[Random.Range(0, gunStats.reloadSound.Length)], gunStats.reloadSoundVol);

        yield return new WaitForSeconds(gunStats.reloadTime);

        if (reserveAmmo >= gunStats.magazineSize)
        {
            currentAmmo = gunStats .magazineSize;
            reserveAmmo -= gunStats.magazineSize;
        }
        else
        {
            currentAmmo = reserveAmmo;
            reserveAmmo = 0;
        }

        isReloading = false;
        Debug.Log(gunStats.gunName + " is reloaded.");
        GameManager.instance.updateAmmoCount(currentAmmo);
    }

    public void TryShoot()
    {
        if (isReloading || GameManager.instance.isPaused)
        {
            return;
        }
        else if (currentAmmo <= 0)
        {
            aud.PlayOneShot(gunStats.gunEmptySound[Random.Range(0, gunStats.gunEmptySound.Length)], gunStats.gunEmptySoundVol);
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
        currentAmmo--;
        GameManager.instance.updateAmmoCount(currentAmmo);

        // recoil
        //playerController.ApplyRecoil(gunStats);
        // muzzleFlash
        StartCoroutine(flashMuzzleFire());
        // shootSound

        Debug.Log(gunStats.gunName + " shot! Bullets left: " + currentAmmo);
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


}
