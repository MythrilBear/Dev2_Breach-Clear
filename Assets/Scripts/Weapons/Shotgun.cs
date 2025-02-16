using UnityEngine;

public class Shotgun : Gun
{
    public int pelletsPerShot = 6;
    public float shotSpread = 5.0f;

    public override void Shoot()
    {
        for ( int i = 0; i < pelletsPerShot; i++)
        {
            RaycastHit hit;

            Vector3 targetPos = cameraTransform.position + cameraTransform.forward * gunStats.shootingRange;

            targetPos = new Vector3(targetPos.x + Random.Range(-shotSpread, shotSpread),
                                    targetPos.y + Random.Range(-shotSpread, shotSpread),
                                    targetPos.z + Random.Range(-shotSpread, shotSpread));

            Vector3 direction = targetPos - cameraTransform.position;
            direction = Vector3.Normalize(direction);

            if (Physics.Raycast(cameraTransform.position, direction,
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
