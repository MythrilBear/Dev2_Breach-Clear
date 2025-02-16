using UnityEngine;

public class SMG : Gun
{

    public override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out hit, gunStats.shootingRange, gunStats.targetLayerMask))
        {
            Debug.Log(gunStats.gunName + " hit " + hit.collider.name);
            Instantiate(gunStats.hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(gunStats.shootDamage);
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            TryShoot();
        }

        if (Input.GetButtonDown("Reload"))
        {
            tryReload();
        }
    }
}
