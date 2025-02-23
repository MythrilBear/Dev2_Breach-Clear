using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPickup, IOpen, IStamina
{
    [Header("----- Components -----")]

    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] public Transform playerCamera;

    [Header("----- Stats -----")]

    [Range(1, 10)] [SerializeField] int HP;
    [Range(1, 10)][SerializeField] int Stam;
    [Range(1, 10)] [SerializeField] float speed;
    [Range(1, 5)] [SerializeField] int sprintMod;
    [Range(1, 2)] [SerializeField] int jumpMax;
    [Range(5, 20)] [SerializeField] int jumpSpeed;
    [Range(15, 45)] [SerializeField] int gravity;

    [Header("----- Crouch -----")]

    [SerializeField] float crouchHeight;
    [SerializeField] float crouchSpeedMod;

    [Header("----- Guns-----")]

    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] GameObject gunModel;
    //[SerializeField] GameObject muzzleFlash;
    [SerializeField] public int currentAmmoCount;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    private Vector3 targetRecoil = Vector3.zero;
    public Vector3 currentRecoil = Vector3.zero;

    [Header("----- Melee -----")]
    
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeRange;
    [SerializeField] GameObject knifeModel;


    [Header("----- Audio -----")]

    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    Vector3 moveDir;

    Vector3 playerVelocity;

    Vector3 originalScale;

    bool isShooting;

    int jumpCount;

    int HPOriginal;
    int StamOrig;

    int gunListPos;
    float shootTimer;

    float originalSpeed;
    float originalHeight;
    float originalCameraY;

    bool isSprinting;
    bool isPlayingSteps;
    bool isCrouching;
    bool isReloading;



    //Additions from Branch PA

    float defusalRange;
    private BombDefusal currentBomb;

    float extractionRange;
    private IntelExtraction currentIntel;

    float moveSpeed;
    float climbSpeed;
    private Rigidbody rb;
    bool isClimbing;
    bool isOnLadder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;
        StamOrig = Stam;
        shootTimer = shootRate;
        originalSpeed = speed;
        originalHeight = controller.height;
        originalCameraY = playerCamera.localPosition.y;
        originalScale = transform.localScale;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
       
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        if(!GameManager.instance.isPaused)
        {
            movement();
            cameraController.instance.currentRecoil = currentRecoil;
            shootTimer += Time.deltaTime;
        }
        ToggleCrouch();
        sprint();

        // Update mission objective.


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

        
        
        // WASD movement.
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        // Vertical movement
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

        //if (Input.GetButton("Shoot") && gunList.Count > 0 && shootTimer >= shootRate && !isReloading)
        //{
        //    if (gunList[gunListPos].ammoCur > 0)
        //    {
        //        shoot();
        //    }
        //    else
        //    {
        //        aud.PlayOneShot(gunList[gunListPos].gunEmptySound[Random.Range(0, gunList[gunListPos].gunEmptySound.Length)], gunList[gunListPos].gunEmptySoundVol);
        //    }
            
        //} 

        //if(Input.GetButtonDown("Melee"))
        //{
        //    meleeAttack();
        //}
        //if(Input.GetButtonDown("Reload"))
        //{
        //    StartCoroutine(reloadGun());
        //}
        //if (Input.GetButtonDown("Aim"))
        //{
        //    Camera.main.fieldOfView = 50f;
        //}
        //if (Input.GetButtonUp("Aim"))
        //{
        //    Camera.main.fieldOfView = 60f;
        //}
    }

    public void ApplyRecoil(gunStats stats)
    {
        float recoilX = Random.Range(-stats.maxRecoil.x, stats.maxRecoil.x) * stats.recoilAmount;
        float recoilY = Random.Range(-stats.maxRecoil.y, stats.maxRecoil.y) * stats.recoilAmount;

        if (recoilY > 0)
        {
            recoilY *= -1;
        }

        targetRecoil += new Vector3(recoilX, recoilY, 0);

        currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, Time.deltaTime * stats.recoilSpeed);

    }

    public void ResetRecoil(gunStats stats)
    {
        currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime * stats.resetRecoilSpeed);
        targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime * stats.resetRecoilSpeed);
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
        if (isCrouching) return;

        if (Input.GetButtonDown("Sprint") && Stam > 0)
        {
            speed *= sprintMod;
            isSprinting = true;
            StartCoroutine(DecreaseStaminaOverTime());
        }
        else if (Input.GetButtonUp("Sprint") || Stam <= 0)
        {
            speed = originalSpeed;
            isSprinting = false;
            StartCoroutine(RecoverStaminaOverTime());
        }
    }

    IEnumerator DecreaseStaminaOverTime()
    {
        while (isSprinting && Stam > 0)
        {
            if (moveDir.magnitude > 0)
            {
                useStamina(1);
            }
            yield return new WaitForSeconds(1f); // Adjust the interval as needed
        }
    }

    IEnumerator RecoverStaminaOverTime()
    {
        while (!isSprinting && Stam < StamOrig)
        {
            recoverStamina(1);
            yield return new WaitForSeconds(1f); // Adjust the interval as needed
        }
    }

    void jump()
    {
        if (isCrouching) return;

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            useStamina(2);
            StartCoroutine(RecoverStaminaOverTime());
        }
        
    }
    
    public void useStamina(int amount)
    {
        Stam -= amount;
        if (Stam <= 0)
        {
           Stam = 0;
        }
        updatePlayerUI();
    }


    public void recoverStamina(int amount)
    {
        Stam += amount;
        if (Stam > StamOrig)
        {
            Stam = StamOrig;
        }
        updatePlayerUI();
    }


    void ToggleCrouch()
    {
        if(Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            Vector3 cameraPosition = playerCamera.localPosition;
            if(isCrouching)
            {
                transform.localScale = new Vector3(originalScale.x, crouchHeight / originalHeight, originalScale.z);
                cameraPosition.y = originalCameraY * (crouchHeight / originalHeight);
                speed = crouchSpeedMod;
            }
            else
            {
                transform.localScale = originalScale;
                cameraPosition.y = originalCameraY;
                speed = originalSpeed;
            }
            playerCamera.localPosition = cameraPosition;
        }
    }

    //void shoot()
    //{
    //    gunList[gunListPos].ammoCur--;
    //    updatePlayerUI();

    //    shootTimer = 0;

    //    aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootSoundVol);

    //    StartCoroutine(flashMuzzleFire());

    //    RaycastHit hit;

    //    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreMask))
    //    {
    //        Debug.Log(hit.collider.name);

    //        Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

            

    //        IDamage dmg = hit.collider.GetComponent<IDamage>();

    //        if (dmg != null)
    //        {
    //            dmg.takeDamage(shootDamage);
    //        }
    //    }
    //}

    //IEnumerator reloadGun()
    //{
    //    isReloading = true;
    //    aud.PlayOneShot(gunList[gunListPos].reloadSound[Random.Range(0, gunList[gunListPos].reloadSound.Length)], gunList[gunListPos].reloadSoundVol);
    //    yield return new WaitForSeconds(gunList[gunListPos].reloadSound.Length);
    //    gunList[gunListPos].ammoCur = gunList[gunListPos].ammoMax;
    //    isReloading = false;
    //}
    //void meleeAttack()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, meleeRange, ~ignoreMask))
    //    {
    //        Debug.Log(hit.collider.name);
    //        IDamage dmg = hit.collider.GetComponent<IDamage>();
    //        if (dmg!=null)
    //        {
    //            dmg.takeDamage(meleeDamage);
    //        }
    //    }
    //}

    //IEnumerator flashMuzzleFire()
    //{
    //    muzzleFlash.SetActive(true);
    //    yield return new WaitForSeconds(0.05f);
    //    muzzleFlash.SetActive(false);
    //}

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
        updatePlayerUI();
        HP += amount;
        if (HP > HPOriginal)
        {
            HP = HPOriginal;
        }
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
        GameManager.instance.PlayerStamBar.fillAmount = (float)Stam / StamOrig;

        //if (gunList.Count > 0)
        //{
        //    GameManager.instance.updateAmmoCount(gunList[gunListPos].ammoCur);
        //}


    }

    public Vector3 getCurrentRecoil()
    {
        return currentRecoil;
    }


    //public void getGunStats(gunStats gun)
    //{
    //    gunList.Add(gun);
    //    gunListPos = gunList.Count - 1;
    //    changeGun();

    //}
    //void selectGun()
    //{
    //    updatePlayerUI();
    //    if(Input.GetAxis("Mouse ScrollWheel")>0 && gunListPos<gunList.Count-1)
    //    {
    //        gunListPos++;
    //        changeGun();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
    //    {
    //        gunListPos--;
    //        changeGun();
    //    }

    //}
    //void changeGun()
    //{
    //    //change the stats
    //    shootDamage = gunList[gunListPos].shootDamage;
    //    shootDistance = gunList[gunListPos].shootDist;
    //    shootRate = gunList[gunListPos].shootRate;

    //    //change model
    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;

    //}

    //public void getMeleeStats(MeleeStats melee)
    //{
    //    meleeDamage = melee.damage;
    //    meleeRange = melee.attackRate;

    //    knifeModel.GetComponent<MeshRenderer>().enabled = !knifeModel.GetComponent<MeshRenderer>().enabled;

    //    //knifeModel.GetComponent<MeshFilter>().sharedMesh = melee.model.GetComponent<MeshFilter>().sharedMesh;
    //    //knifeModel.GetComponent<MeshRenderer>().sharedMaterial = melee.model.GetComponent<MeshRenderer>().sharedMaterial;
    //}

    //Additions from Branch PA

    void CheckForBombDefusal()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentBomb != null)
        {
            float distanceToBomb = Vector3.Distance(transform.position, currentBomb.transform.position);
            if (distanceToBomb <= defusalRange)
            {
                currentBomb.StartDefusal();
            }
        }
    }

    void CheckForIntelExtraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentIntel != null)
        {
            float distancetoIntel = Vector3.Distance(transform.position, currentIntel.transform.position);
            if (distancetoIntel <= extractionRange)
            {
                currentIntel.StartExtraction();
            }
        }
    }

    void CheckForLadder()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (isClimbing)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += verticalInput * climbSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        else
        {
            Vector3 newPosition = transform.position;
            newPosition.x += horizontalInput * moveSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        if (isOnLadder && Mathf.Abs(verticalInput) > 0)
        {
            isClimbing = true;
            rb.useGravity = false;
        }
        else if (!isOnLadder)
        {
            isClimbing = false;
            rb.useGravity = true;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Bomb"))
    //    {
    //        currentBomb = other.GetComponent<BombDefusal>();
    //    }
    //    else if (other.CompareTag("Intel Spot"))
    //    {
            
    //    }
    //    else if (other.CompareTag("Ladder"))
    //    {
    //        isOnLadder = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Bomb"))
    //    {
    //        currentBomb = null;
    //    }
    //    else if (other.CompareTag("Intel Spot"))
    //    {
    //        currentIntel = null;
    //    }
    //    else if (other.CompareTag("Ladder"))
    //    {
    //        isOnLadder = false;
    //    }
    //}
}
