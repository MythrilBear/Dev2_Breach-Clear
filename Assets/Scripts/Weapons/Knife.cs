using UnityEngine;

public class Knife : Gun
{
    public AudioSource audSource;
    //public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out hit, gunStats.shootingRange, gunStats.targetLayerMask))
        {
           // Debug.Log(gunStats.gunName + " hit " + hit.collider.name);
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

        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
            audSource.PlayOneShot(gunStats.shootSound[Random.Range(0, gunStats.shootSound.Length)], gunStats.shootSoundVol);

            GetComponent<Animator>().Play("KnifeSwing");
        }

        
    }
}
