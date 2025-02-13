using UnityEngine;

public class Rifle : Gun
{

    public override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out hit, gunStats.shootingRange, gunStats.targetLayerMask))
        {
            Debug.Log(gunStats.gunName + " hit " + hit.collider.name);
            Instantiate(gunStats.hitEffect, hit.point, Quaternion.identity);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Shoot"))
        {
            TryShoot();
        }

        if (Input.GetButtonDown("Reload"))
        {
            tryReload();
        }
    }
}
