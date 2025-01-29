using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPickup, IOpen
{
    [Header("----- Components -----")]


    [SerializeField] CharacterController controller;

    [SerializeField] AudioSource aud;

    [SerializeField] LayerMask ignoreMask;

    [Header("----- Stats -----")]

    [Range(1, 10)] [SerializeField] int HP;

    [Range(1, 10)] [SerializeField] int speed;

    [Range(1, 5)] [SerializeField] int sprintMod;

    [Range(1, 2)] [SerializeField] int jumpMax;

    [Range(5, 20)] [SerializeField] int jumpSpeed;

    [Range(15, 45)] [SerializeField] int gravity;

    [Header("----- Guns-----")]

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    [SerializeField] GameObject gunModel;

    [SerializeField] GameObject muzzleFlash;

    [SerializeField] int shootDamage;

    [SerializeField] int shootDistance;

    [SerializeField] float shootRate;

    [Header("----- Audio -----")]

    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    Vector3 moveDir;

    Vector3 playerVelocity;

    bool isShooting;


    int jumpCount;

    int HPOriginal;

    int gunListPos;
    float shootTimer;

    bool isSprinting;
    bool isPlayingSteps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;
        updatePlayerUI();
        shootTimer = shootRate;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        if(!GameManager.instance.isPaused)
        {
            movement();
            selectGun();
            shootTimer += Time.deltaTime;
        }
        sprint();
    }

    void movement()
    {
        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //transform.position += moveDir * speed * Time.deltaTime;

        if (controller.isGrounded)
        {
            if (moveDir.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }

            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                    (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Shoot") && gunList.Count > 0 && shootTimer >= shootRate)
        {
            shoot();
            
        } 
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
        if (!isSprinting)
        {

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        isPlayingSteps = false;
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        StartCoroutine(flashMuzzleFire());

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);

            Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

            

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    IEnumerator flashMuzzleFire()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);

        updatePlayerUI();

        StartCoroutine(flashDamagePanel());

        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    public void recoverHealth(int amount)
    {
        HP += amount;
        if (HP > HPOriginal)
        {
            HP = HPOriginal;
        }
        updatePlayerUI();
    }

    IEnumerator flashDamagePanel()
    {
        GameManager.instance.damagePanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.damagePanel.SetActive(false);
    }

    void updatePlayerUI()
    {
        GameManager.instance.PlayerHPBar.fillAmount = (float)HP / HPOriginal;
    }
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;
        changeGun();

    }
    void selectGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel")>0 && gunListPos<gunList.Count-1)
        {
            gunListPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }

    }
    void changeGun()
    {
        //change the stats
        shootDamage = gunList[gunListPos].shootDamage;
        shootDistance = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;

        //change model
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;

    }
}
