using System.Collections;
using UnityEngine;

public class Grenade : Gun
{
    
    private bool isCharging = false;
    private float chargeTime = 0f;
    

    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab;

    [Header("Grenade Settings")]
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0);

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxForce = 20f;

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Shoot()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
        //    out hit, gunStats.shootingRange, gunStats.targetLayerMask))
        //{
        //    Debug.Log(gunStats.gunName + " hit " + hit.collider.name);
        //    Instantiate(gunStats.hitEffect, hit.point, Quaternion.identity);
        //}

        ReleaseThrow();
        Debug.Log("throw has been released");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        if (Input.GetButtonDown("Shoot") && currentAmmo > 0)
        {
            StartThrowing();
            //grenadePrefab.SetActive(false);
        }

        if (isCharging)
        {
            ChargeThrow();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            TryShoot();
            Debug.Log("Button has been released");
        }

        
    }

    

    void StartThrowing()
    {
        // pull pin sound
        isCharging = true;
        chargeTime = 0f;

        // trajectory line
        trajectoryLine.enabled = true;
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        // trajectory line velocity
        Vector3 grenadeVelocity = (cameraTransform.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * throwForce, maxForce);
        ShowTrajectory(throwPosition.position, grenadeVelocity);
    }

    void ReleaseThrow()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        isCharging = false;

        trajectoryLine.enabled = false;
    }

    public void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position + cameraTransform.transform.forward;

        GameObject grenade =  Instantiate(grenadePrefab, spawnPosition, cameraTransform.transform.rotation);

        Debug.Log("the grenade has been created");

        Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();

        Vector3 finalThrowDirection = (cameraTransform.transform.forward + throwDirection).normalized;
        
        rigidbody.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];

        trajectoryLine.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;

            // formula for displacement
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;

        }
        trajectoryLine.SetPositions(points);
    }    

}
