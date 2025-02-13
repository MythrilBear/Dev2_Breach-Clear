using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public gunStats gunStats;
    public playerController playerController;
    public Transform cameraTransform;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunStats.magazineSize;

        playerController = transform.root.GetComponent<playerController>();
        cameraTransform = playerController.playerCamera.transform;
    }

    public void tryReload()
    {
        if (!isReloading && currentAmmo < gunStats.magazineSize && gunStats.reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunStats.gunName + " is reloading...");

        yield return new WaitForSeconds(gunStats.reloadTime);

        if (gunStats.reserveAmmo >= gunStats.magazineSize)
        {
            currentAmmo = gunStats .magazineSize;
            gunStats.reserveAmmo -= gunStats.magazineSize;
        }
        else
        {
            currentAmmo = gunStats.reserveAmmo;
            gunStats.reserveAmmo = 0;
        }

        isReloading = false;
        Debug.Log(gunStats.gunName + " is reloaded.");
    }
}
